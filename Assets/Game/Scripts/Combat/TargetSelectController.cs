using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Combat
{
    public class TargetSelectController : MonoBehaviour
    {
        [field: SerializeField] public bool IsActive { get; private set; }
        [field: SerializeField] public Collider2D Target { get; private set; }
        [field: SerializeField] public bool InMemory { get; private set; }
        
        [Header("Settings")]
        [Min(0f)]
        [SerializeField] private float bodyRadius = 0.5f;
        [Min(0f)]
        [SerializeField] private float radiusSearch = 5f;
        [SerializeField] private LayerMask targetLayer = 1;
        [SerializeField] private LayerMask obstacleLayer = 1;

        [Space]
        [SerializeField] private float memoryDuration = 5f;
        
        private readonly RaycastHit2D[] targetCast = new RaycastHit2D[4];
        private readonly RaycastHit2D[] obstacleCast = new RaycastHit2D[1];
        
        private ContactFilter2D targetFilter2D;
        private ContactFilter2D obstacleFilter2D;
        
        private float lastTimeHasTarget;
        
        public Vector2 OriginPosition => transform.position;
        public bool HasTarget => Target != null;
        
        public void Begin()
        {
            if (IsActive) return;
            IsActive = true;
        }

        public void End()
        {
            if (!IsActive) return;
            IsActive = true;
        }

        public void SetTarget(Collider2D newTarget)
        {
            if (newTarget.gameObject.ContainsMask(targetLayer))
            {
                Target = newTarget;
                lastTimeHasTarget = Time.time;
            }
        }
        
        private bool TryGetTarget(out Collider2D targetCollider)
        {
            targetCollider = null;
            
            var countTargets = Physics2D.CircleCast(OriginPosition, radiusSearch, Vector2.zero, targetFilter2D, targetCast);

            for (var i = 0; i < countTargets; i++)
            {
                var target = targetCast[i];

                if (target.collider.attachedRigidbody && target.collider.attachedRigidbody.TryGetComponent(out HealthController health))
                {
                    if (health.IsDead) continue;
                }
                
                var direction = target.point - OriginPosition;

                var countObstacle = Physics2D.CircleCast(OriginPosition, bodyRadius, direction, obstacleFilter2D, obstacleCast, radiusSearch);

                if (countObstacle != 1) continue;

                if (target.collider.attachedRigidbody)
                {
                    if (obstacleCast[0].collider.attachedRigidbody != target.collider.attachedRigidbody) continue;
                }
                else
                {
                    if (obstacleCast[0].collider != target.collider) continue;
                }
                
                // Target!!!
                targetCollider = target.collider;
                lastTimeHasTarget = Time.time;
                return true;
            }

            if (Target != null)
            {
                var timeSinceForget = Time.time - lastTimeHasTarget;
                if (timeSinceForget < memoryDuration)
                {
                    targetCollider = Target;
                    InMemory = true;
                    return true;
                }
            }
            
            InMemory = false;
            return false;
        }
        
        private void Awake()
        {
            targetFilter2D = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = targetLayer,
                useTriggers = false,
            };
            
            obstacleFilter2D = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = obstacleLayer | targetLayer,
                useTriggers = false,
            };
        }
        
        private void FixedUpdate()
        {
            if (!IsActive) return;

            Target = TryGetTarget(out var target) ? target : null;
        }
    }
}