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
        Harvest
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

        public void SetUpPlayer(int playerId){
            if(playerId >= playerSkins.Length)
                playerId = 0;

                animator = playerSkins[playerId].Animator;
                playerSkins[playerId].gameObject.SetActive(true);            
        }
        private void OnForwardedPlayerTriggerEnter(Collider2D collider)
        {
            PerformFunctionOn(AddAndShow, collider, weedRootsWithinRange);
            PerformFunctionOn(AddAndShow, collider, seedlingsWithinRange);
            PerformFunctionOn(AddAndShow, collider, waterWithinRange);
        }
        private void OnForwardedAoeTriggerEnter(Collider2D collider)
        {
            WeedRoot[] weedRoots = collider.GetComponentsInParent<WeedRoot>();
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
            if (weedRootsWithinRange.Count <= 0 &&
                seedlingsWithinRange.Count <= 0 &&
                waterWithinRange.Count <= 0)
                OnInteract(null);

            else if (weedRootsWithinRange.Count > 0)
                animator.SetTrigger("TriggerHit");

            else if (seedlingsWithinRange.Count > 0)
                animator.SetBool("Watering", true);

            foreach (var seedling in seedlingsWithinRange)
                OnInteract(seedling);

            foreach (var water in waterWithinRange)
                OnInteract(water);

            foreach (WeedRoot weedRoot in weedRootsWithinRange)
                OnInteract(weedRoot);
        }

        private void FixedUpdate() => AddRotation(currentSpeed);

        private void Update()
        {
            switch (CurrentlyHolding)
            {
                case Cargo.Nothing:
                    // cargoImage.image = null;
                    break;
                case Cargo.Water:
                    // cargoImage.sprite = Configs.Instance.Get.waterSprite;
                    break;
                case Cargo.Flower:
                    // cargoImage.sprite = Configs.Instance.Get.flowerSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (RemainingInteractionTime > 0)
            {
                cargoImage.fillAmount = CalculatePercentage(RemainingInteractionTime, FullInteractionTime);
                RemainingInteractionTime -= Time.deltaTime;
            }
        }

        public static float CalculatePercentage(float currentTime, float totalTime)
        {
            float calculatePercentage = (100f- (currentTime / totalTime * 100))/100f;
            Debug.Log($"%%% {calculatePercentage}");
            return calculatePercentage;
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
            aoeRadius.enabled = false;
        }
        public void PerformInteraction(float duration, InteractionType interactionType)
        {
            RemainingInteractionTime = duration;
            FullInteractionTime = duration;
            cargoImage.enabled = true;

            switch (interactionType)
            {
                case InteractionType.Seed:
                    cargoImage.sprite = Configs.Instance.Get.leafSprite;
                    break;
                case InteractionType.Water:
                    cargoImage.sprite = Configs.Instance.Get.waterSprite;
                    break;
                case InteractionType.Harvest:
                    cargoImage.sprite = Configs.Instance.Get.flowerSprite;
                    break;
            }
        }
        public void HideCargoUI()
        {
            cargoImage.enabled = false;
        }
    }
}