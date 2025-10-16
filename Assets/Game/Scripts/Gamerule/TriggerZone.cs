using System;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Gamerule
{
    public class TriggerZone : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask = 1;
        
        public event Action<Collider2D> Triggered;

        public bool IsValidCollider(Collider2D cld)
        {
            if (cld.attachedRigidbody)
            {
                return cld.attachedRigidbody.gameObject.ContainsMask(layerMask);
            }
            
            return cld.gameObject.ContainsMask(layerMask);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsValidCollider(other))
            {
                Triggered?.Invoke(other);
            }
        }
    }
}