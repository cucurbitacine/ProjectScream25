using System;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Combat
{
    public class AttackController : MonoBehaviour
    {
        [field: SerializeField] public bool IsAttacking { get; set; }
        [field: SerializeField] public float LastAttackTime { get; set; }
        [field: SerializeField] public int Team { get; set; } = 0;

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

        public GizmosData gizmosData;
        
        private void OnDrawGizmos()
        {
            //if (IsAttacking)
            {
                Gizmos.color = Color.softRed;
                Gizmos.DrawSphere(gizmosData.center, gizmosData.radius);
            }
        }
    }

    [Serializable]
    public struct GizmosData
    {
        public Vector2 center;
        public float radius;
    }
}