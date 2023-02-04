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
        private string name = "";
        private int currentSpeed;


        void Awake()
        {
            playerInput = GetComponent<PlayerInputController>();
        }

        void OnEnable()
        {
            playerInput.OnMove += HandleMove;
        }

        private void OnDisable()
        {            
            playerInput.OnMove -= HandleMove;
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