using System;
using System.Collections.Generic;
using UnityEngine;
namespace DefaultNamespace
{
    public class Player : Rotatable
    {
        private int movementSpeed = 10;

        [SerializeField] private PlayerInputController playerInput;
        [SerializeField] private int maxSpeed = 5;
        [SerializeField] private ColliderForwarder colliderForwarder;
        public event Action<InteractiveRotatable> OnInteract;

        private string name = "";
        private int currentSpeed;
        private List<WeedRoot> weedRootsWithinRange = new();
        private List<Seedling> seedlingsWithinRange = new();

        void Awake()
        {
            playerInput = GetComponent<PlayerInputController>();
        }

        void OnEnable()
        {
            colliderForwarder.OnTriggerEnterForward += OnForwardedTriggerEnterForward;
            colliderForwarder.OnTriggerExitForward += OnForwardedTriggerExit;
            playerInput.OnMove += HandleMove;
            playerInput.OnAction += HandleAction;
        }

        private void OnDisable()
        {
            colliderForwarder.OnTriggerEnterForward -= OnForwardedTriggerEnterForward;
            colliderForwarder.OnTriggerExitForward -= OnForwardedTriggerExit;
            playerInput.OnMove -= HandleMove;
            playerInput.OnAction -= HandleAction;
        }
        private void OnForwardedTriggerEnterForward(Collider2D collider)
        {
            PerformFunctionOn(AddAndShow, collider, weedRootsWithinRange);
            PerformFunctionOn(AddAndShow, collider, seedlingsWithinRange);
            PrintWhatIsInRange();
        }
        private void OnForwardedTriggerExit(Collider2D collider)
        {
            PerformFunctionOn(RemoveAndHide, collider, weedRootsWithinRange);
            PerformFunctionOn(RemoveAndHide, collider, seedlingsWithinRange);
            PrintWhatIsInRange();
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
            if (weedRootsWithinRange.Count <= 0 && seedlingsWithinRange.Count <= 0)
                OnInteract(null);

            foreach (var seedling in seedlingsWithinRange)
                OnInteract(seedling);

            foreach (WeedRoot weedRoot in weedRootsWithinRange)
                weedRoot.RipOut(25);
        }
        private void PrintWhatIsInRange()
        {
            Debug.Log(string.Join(" ", weedRootsWithinRange));
            Debug.Log(string.Join(" ", seedlingsWithinRange));
        }

        private void FixedUpdate()
        {
            AddRotation(currentSpeed);
        }
        private void HandleMove(float speed)
        {
            currentSpeed = (int)(speed * maxSpeed);
        }
    }
}