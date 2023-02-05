using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace DefaultNamespace
{
    public enum Cargo
    {
        Nothing,
        Water,
        Flower
    }
    public class Player : Rotatable
    {
        private int movementSpeed = 10;

        [SerializeField] private PlayerInputController playerInput;
        [SerializeField] private ColliderForwarder playerColliderForwarder;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer cargoSprite;

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

            else if(weedRootsWithinRange.Count > 0)
                animator.SetTrigger("TriggerHit");

            else if(seedlingsWithinRange.Count > 0)
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
                    cargoSprite.sprite = null;
                    break;
                case Cargo.Water:
                    cargoSprite.sprite = Configs.Instance.Get.waterSprite;
                    break;
                case Cargo.Flower:
                    cargoSprite.sprite = Configs.Instance.Get.flowerSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void HandleMove(float speed)
        {
            currentSpeed = (speed * Configs.Instance.Get.maxSpeed);
            animator.SetFloat("Speed", currentSpeed);
            animator.SetBool("Watering", false);
            //Debug.Log("HandleMove speed: " + speed + " currentSpeed: " + currentSpeed);
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
    }
}