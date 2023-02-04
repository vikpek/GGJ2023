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

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private int maxSpeed = 5;
        [SerializeField] private ColliderForwarder colliderForwarder;

        public event Action<WeedRoot> IsOnWeedRoot;

        private string name = "";
        private int currentSpeed;


        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        void OnEnable()
        {
            colliderForwarder.OnCollision += OnCollision;
            playerInput.OnMove += HandleMove;
        }

        private void OnDisable()
        {
            colliderForwarder.OnCollision -= OnCollision;
            playerInput.OnMove -= HandleMove;
        }
        private void OnCollision(Collider obj)
        {

        }

        private void FixedUpdate()
        {
            AddRotation(currentSpeed);
        }
                public void MoveCounterClockwise()
        {
            AddRotation(movementSpeed);
        }
        private void HandleMove(float speed)
        {
            //Debug.Log("Handle");
            currentSpeed = (int)(speed * maxSpeed);
        }


    }
}