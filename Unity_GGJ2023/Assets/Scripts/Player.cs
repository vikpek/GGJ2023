using System;
using UnityEngine;
using UnityEditor;

namespace DefaultNamespace
{
    public class Player : Rotatable
    {
        private int movementSpeed = 10;
        public void MoveClockwise()
        {
            AddRotation(movementSpeed);
        }

        [SerializeField] private PlayerInputController playerInput;
        [SerializeField] private int maxSpeed = 5;
        [SerializeField] private ColliderForwarder colliderForwarder;
        public event Action<WeedRoot> IsOnWeedRoot;

        private string name = "";
        private int currentSpeed;
        private WeedRoot[] StoodOnWeedRoot = null;

        void Awake()
        {
            playerInput = GetComponent<PlayerInputController>();
        }

        void OnEnable()
        {
            colliderForwarder.OnCollision += OnForwardedCollision;
            playerInput.OnMove += HandleMove;
        }

        private void OnDisable()
        {
            colliderForwarder.OnCollision -= OnForwardedCollision;
            playerInput.OnMove -= HandleMove;
        }
        private void OnForwardedCollision(Collider obj)
        {
            WeedRoot[] weedRoot = obj.GetComponentsInParent<WeedRoot>();
            if (weedRoot is null)
                return;

            StoodOnWeedRoot = weedRoot;
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