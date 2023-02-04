using System;
using UnityEngine;
namespace DefaultNamespace
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ColliderForwarder : MonoBehaviour
    {
        public event Action<Collider> OnCollision;
        private void OnTriggerEnter(Collider other) => OnCollision(other);
    }
}