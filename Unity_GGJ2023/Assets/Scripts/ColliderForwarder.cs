using System;
using UnityEngine;
namespace DefaultNamespace
{
    public class ColliderForwarder : MonoBehaviour
    {
        public event Action<Collider2D> OnTriggerEnterForward;
        public event Action<Collider2D> OnTriggerExitForward;
        private void OnTriggerEnter2D(Collider2D other) => OnTriggerEnterForward(other);
        private void OnTriggerExit2D(Collider2D other) => OnTriggerExitForward(other);
    }
}