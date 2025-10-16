using System;
using Game.Scripts.Combat;
using Game.Scripts.Sound;
using UnityEngine;

namespace Game.Scripts.Player
{
    [RequireComponent(typeof(HealthController))]
    public class Damageable : MonoBehaviour
    {
        public HealthController Health { get; private set; }
        
        [Header("SFX")]
        [SerializeField] private SoundSource soundSource;
        [SerializeField] private SoundFxPreset damageSfx;
        
        private Hitbox[] hitboxes;
        
        private void OnDamaged(int amount)
        {
            Health.Damage(amount);

            if (amount > 0)
            {
                if (soundSource && damageSfx)
                {
                    soundSource.Play(damageSfx);
                }
            }
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