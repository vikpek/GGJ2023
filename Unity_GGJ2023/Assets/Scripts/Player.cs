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
        public event Action<WeedRoot> IsOnWeedRoot;

        private string name = "";
        private int currentSpeed;
        private List<WeedRoot> rootsWithinRange = new();

        void Awake()
        {
            playerInput = GetComponent<PlayerInputController>();
        }

        void OnEnable()
        {
            colliderForwarder.OnTriggerEnterForward += OnForwardedTriggerEnterForward;
            colliderForwarder.OnTriggerExitForward += OnForwardedTriggerExit;
            playerInput.OnMove += HandleMove;
        }

        private void OnDisable()
        {
            colliderForwarder.OnTriggerEnterForward -= OnForwardedTriggerEnterForward;
            colliderForwarder.OnTriggerExitForward -= OnForwardedTriggerExit;
            playerInput.OnMove -= HandleMove;
        }
        private void OnForwardedTriggerEnterForward(Collider2D collider)
        {
            WeedRoot[] weedRoots = collider.GetComponentsInParent<WeedRoot>();
            if (weedRoots is null)
                return;

            rootsWithinRange.AddRange(weedRoots);
            PrintCurrentRootsInRange();
        }
        private void OnForwardedTriggerExit(Collider2D collider)
        {
            WeedRoot[] weedRoots = collider.GetComponentsInParent<WeedRoot>();
            if (weedRoots is null)
                return;

            foreach (WeedRoot weedRoot in weedRoots)
            {
                if(rootsWithinRange.Contains(weedRoot))
                    rootsWithinRange.Remove(weedRoot);
            }

            PrintCurrentRootsInRange();
        }
        private void PrintCurrentRootsInRange()
        {
            Debug.Log(string.Join(" ", rootsWithinRange));
        }

        private void FixedUpdate()
        {
            AddRotation(currentSpeed);
        }
        private void HandleMove(float speed)
        {
            //Debug.Log("Handle");
            currentSpeed = (int)(speed * maxSpeed);
        }
    }
}