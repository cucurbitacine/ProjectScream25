using System;
using Game.Scripts.Combat;
using UnityEngine;

namespace Game.Scripts.Player
{
    [RequireComponent(typeof(HealthController))]
    public class Damageable : MonoBehaviour
    {
        public HealthController Health { get; private set; }
        
        private Hitbox[] hitboxes;
        
        private void OnDamaged(int amount)
        {
            Health.Damage(amount);
        }

        private void Awake()
        {
            hitboxes = GetComponentsInChildren<Hitbox>();
            Health = GetComponent<HealthController>();
        }

        private void OnEnable()
        {
            foreach (var hitbox in hitboxes)
            {
                hitbox.Damaged += OnDamaged;
            }
        }

        private void OnDisable()
        {
            foreach (var hitbox in hitboxes)
            {
                hitbox.Damaged -= OnDamaged;
            }
        }
    }
}