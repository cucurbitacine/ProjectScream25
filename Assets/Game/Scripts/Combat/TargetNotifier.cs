using UnityEngine;

namespace Game.Scripts.Combat
{
    public class TargetNotifier : MonoBehaviour
    {
        [SerializeField] private Collider2D target;

        [Space]
        [Min(0f)]
        [SerializeField] private float radiusNotification = 10f;
        [SerializeField] private LayerMask observerLayer = 1;

        private readonly Collider2D[] observers = new Collider2D[32];
        
        public void Notify()
        {
            var contactFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = observerLayer,
                useTriggers = true,
            };

            var count = Physics2D.OverlapCircle(target.transform.position, radiusNotification, contactFilter, observers);

            for (var i = 0; i < count; i++)
            {
                var observer = observers[i];

                if (observer.TryGetComponent<TargetSelectController>(out var targetSelect))
                {
                    targetSelect.SetTarget(target);
                }
            }
        }
    }
}