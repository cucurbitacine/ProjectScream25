using System;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Combat
{
    public class AttackController : MonoBehaviour
    {
        [field: SerializeField] public bool IsAttacking { get; set; }
        [field: SerializeField] public float LastAttackTime { get; set; }

        [field: Space]
        [field: SerializeField] public LayerMask TargetLayer { get; private set; } = 1;

        private Hitbox[] hitboxes;

        public bool Contains(Hitbox hitbox)
        {
            return hitboxes != null && hitboxes.Contains(hitbox);
        }
        
        private void Awake()
        {
            hitboxes = GetComponentsInChildren<Hitbox>();
        }
    }
}