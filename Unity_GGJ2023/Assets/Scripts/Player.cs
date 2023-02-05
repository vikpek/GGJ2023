using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DefaultNamespace
{
    public enum Cargo
    {
        Nothing,
        Water,
        Flower
    }

    public enum InteractionType
    {
        Seed,
        Water,
        Harvest,
        WaterPickup
    }
    public class Player : Rotatable
    {
        private int movementSpeed = 10;

        [SerializeField] private PlayerInputController playerInput;
        [SerializeField] private ColliderForwarder playerColliderForwarder;
        [SerializeField] private Animator animator;
        [SerializeField] private Image cargoImage;

        [SerializeField] private PlayerSkin[] playerSkins;

        [SerializeField] private ColliderForwarder aoeColliderForwarder;
        [SerializeField] private CircleCollider2D aoeRadius;

        public event Action<InteractiveRotatable> OnInteract;

        private int waterLevel = 0;
        private int flowerAmount = 0;

        private string name = "";
        private float currentSpeed;
        public Cargo CurrentlyHolding;
        private List<WeedRoot> weedRootsWithinRange = new();
        private List<Seedling> seedlingsWithinRange = new();
        private List<Water> waterWithinRange = new();

        private float RemainingInteractionTime = 0f;
        private float FullInteractionTime = 0f;

        private bool inInteractionDelay = false;


        private State state = State.None;
        private InteractionType currentInteraction;

        public enum State { None, Planting, Stomping, Harvesting };


        void Awake()
        {
            playerInput = GetComponent<PlayerInputController>();
        }

        void OnEnable()
        {
            playerColliderForwarder.OnTriggerEnterForward += OnForwardedPlayerTriggerEnter;
            playerColliderForwarder.OnTriggerExitForward += OnForwardedPlayerTriggerExit;

            aoeColliderForwarder.OnTriggerEnterForward += OnForwardedAoeTriggerEnter;

            playerInput.OnMove += HandleMove;
            playerInput.OnAction += HandleAction;
            aoeRadius.enabled = false;
            aoeRadius.radius = Configs.Instance.Get.aoeRadius;
        }

        private void OnDisable()
        {
            playerColliderForwarder.OnTriggerEnterForward -= OnForwardedPlayerTriggerEnter;
            playerColliderForwarder.OnTriggerExitForward -= OnForwardedPlayerTriggerExit;
            aoeColliderForwarder.OnTriggerEnterForward -= OnForwardedAoeTriggerEnter;
            playerInput.OnMove -= HandleMove;
            playerInput.OnAction -= HandleAction;
        }

        public bool InState(State state) => this.state == state;

        public void SwitchState(State state) => this.state = state;

        public void SetUpPlayer(int playerId)
        {
            if (playerId >= playerSkins.Length)
                playerId = 0;

            animator = playerSkins[playerId].Animator;
            playerSkins[playerId].gameObject.SetActive(true);
            StartCoroutine("InteractionDelay");
        }
        public void StartInteractionDelay() => StartCoroutine("InteractionDelay");
        private void OnForwardedPlayerTriggerEnter(Collider2D collider)
        {
            PerformFunctionOn(AddAndShow, collider, weedRootsWithinRange);
            PerformFunctionOn(AddAndShow, collider, seedlingsWithinRange);
            PerformFunctionOn(AddAndShow, collider, waterWithinRange);
        }
        private void OnForwardedAoeTriggerEnter(Collider2D collider)
        {
            Debug.Log("OnForwardedAoeTrigger");
            WeedRoot[] weedRoots = collider.GetComponentsInParent<WeedRoot>();
            Debug.Log("OnForwardedAoeTrigger weedRoots: " + weedRoots);
            if (weedRoots is null)
                return;

            foreach (WeedRoot weedRoot in weedRoots)
                weedRoot.RaiseOnRemove(weedRoot);
        }
        private void OnForwardedPlayerTriggerExit(Collider2D collider)
        {
            PerformFunctionOn(RemoveAndHide, collider, weedRootsWithinRange);
            PerformFunctionOn(RemoveAndHide, collider, seedlingsWithinRange);
            PerformFunctionOn(RemoveAndHide, collider, waterWithinRange);
        }
        bool AddAndShow<T>(List<T> interactiveRotatables, T interactiveRotatable) where T : InteractiveRotatable
        {
            if (!interactiveRotatables.Contains(interactiveRotatable))
            {
                interactiveRotatables.Add(interactiveRotatable);
                interactiveRotatable.ShowActionButtonHint();
                return true;
            }
            return false;
        }
        bool RemoveAndHide<T>(List<T> interactiveRotatables, T interactiveRotatable) where T : InteractiveRotatable
        {
            if (interactiveRotatables.Contains(interactiveRotatable))
            {
                interactiveRotatables.Remove(interactiveRotatable);
                interactiveRotatable.HideActionButtonHint();
            }
            return true;
        }
        private void PerformFunctionOn<T>(Func<List<T>, T, bool> func, Collider2D collider, List<T> interactiveRotatables) where T : InteractiveRotatable
        {
            T[] interactableRotatable = collider.GetComponentsInParent<T>();
            if (interactableRotatable is null)
                return;

            foreach (T interactiveRotatable in interactableRotatable)
                func(interactiveRotatables, interactiveRotatable);
        }
        private void HandleAction()
        {
            if (inInteractionDelay)
                return;

            if (weedRootsWithinRange.Count <= 0 &&
                seedlingsWithinRange.Count <= 0 &&
                waterWithinRange.Count <= 0)
            {
                OnInteract(null);
                StartCoroutine("InteractionDelay");
                return;
            }

            if (weedRootsWithinRange.Count > 0)
            {
                AudioManager.Instance.PlayAudio(ClipPurpose.Stomping);
            }
            else if (seedlingsWithinRange.Count > 0)
            {
                AudioManager.Instance.PlayAudio(ClipPurpose.Planting);
            }

            foreach (var seedling in seedlingsWithinRange)
            {
                OnInteract(seedling);
                StartCoroutine("InteractionDelay");
                return;
            }

            foreach (var water in waterWithinRange)
            {
                OnInteract(water);
                StartCoroutine("InteractionDelay");
                return;
            }

            foreach (WeedRoot weedRoot in weedRootsWithinRange)
            {
                OnInteract(weedRoot);
                StartCoroutine("InteractionDelay");
                return;
            }
        }

        public void RemoveWeedRoots(WeedRoot weedRoot)
        {
            if (weedRootsWithinRange.Contains(weedRoot))
            {
                weedRootsWithinRange.Remove(weedRoot);
            }
        }

        public void RemoveSeedling(Seedling seedling)
        {
            if (seedlingsWithinRange.Contains(seedling))
            {
                seedlingsWithinRange.Remove(seedling);
            }
        }

        private void FixedUpdate() => AddRotation(currentSpeed);

        private void Update()
        {
            if (RemainingInteractionTime > 0)
            {
                if (currentInteraction == InteractionType.Water)
                    cargoImage.fillAmount = CalculationHelper.CalculatePercentage(RemainingInteractionTime, FullInteractionTime);
                else
                    cargoImage.fillAmount = CalculationHelper.CalculatePercentageReverted(RemainingInteractionTime, FullInteractionTime);
                RemainingInteractionTime -= Time.deltaTime;
            }
        }

        private void HandleMove(float speed)
        {
            if (RemainingInteractionTime > 0)
            {
                currentSpeed = 0;
            }
            else
            {
                currentSpeed = (speed * Configs.Instance.Get.maxSpeed);
                animator.SetFloat("Speed", currentSpeed);
                animator.SetBool("Watering", false);
                //Debug.Log("HandleMove speed: " + speed + " currentSpeed: " + currentSpeed);
            }
        }
        public void AddWater()
        {
            CurrentlyHolding = Cargo.Water;
            waterLevel++;
        }
        public bool HasWater() => waterLevel > 0;
        public void UseWater()
        {
            CurrentlyHolding = Cargo.Nothing;
            animator.SetBool("Watering", true);
            waterLevel = 0;
        }
        public void AddFlower()
        {
            CurrentlyHolding = Cargo.Flower;
            flowerAmount++;
        }
        public bool HasFlower() => flowerAmount > 0;
        public void UseFlower()
        {
            CurrentlyHolding = Cargo.Nothing;
            flowerAmount = 0;
        }
        public void AoeBomb()
        {
            aoeRadius.enabled = true;
            StartCoroutine(SkipFrame());
        }
        private IEnumerator SkipFrame()
        {
            yield return null;
            if (aoeRadius != null)
                aoeRadius.enabled = false;
        }

        private IEnumerator InteractionDelay()
        {
            float deltaTime = 0.0f;
            inInteractionDelay = true;
            while (deltaTime <= Configs.Instance.Get.interactionDelay)
            {
                deltaTime += Time.deltaTime;
                yield return null;
            }
            inInteractionDelay = false;
        }
        public void PerformInteraction(float duration, InteractionType interactionType)
        {
            RemainingInteractionTime = duration;
            FullInteractionTime = duration;
            cargoImage.enabled = true;
            currentInteraction = interactionType;
            switch (interactionType)
            {
                case InteractionType.Seed:
                    cargoImage.sprite = Configs.Instance.Get.leafSprite;
                    AudioManager.Instance.PlayAudio(ClipPurpose.Planting);
                    animator.SetFloat("Speed", 0f);
                    break;
                case InteractionType.Water:
                    cargoImage.sprite = Configs.Instance.Get.waterSprite;
                    AudioManager.Instance.PlayAudio(ClipPurpose.Watering);
                    animator.SetBool("Watering", true);
                    animator.SetFloat("Speed", 0f);
                    break;
                case InteractionType.WaterPickup:
                    cargoImage.sprite = Configs.Instance.Get.waterSprite;
                    AudioManager.Instance.PlayAudio(ClipPurpose.Watering);
                    break;
                case InteractionType.Harvest:
                    cargoImage.sprite = Configs.Instance.Get.flowerSprite;
                    AudioManager.Instance.PlayAudio(ClipPurpose.Gathering);
                    animator.SetTrigger("TriggerHit");
                    break;
            }
        }
        public void HideCargoUI()
        {
            cargoImage.enabled = false;
        }
    }
}