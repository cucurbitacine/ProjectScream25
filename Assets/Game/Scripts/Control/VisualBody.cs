using System;
using UnityEngine;

namespace Game.Scripts.Control
{
    public class VisualBody : MonoBehaviour
    {
        public RotateSettings RotateSettings = RotateSettings.Default;

        [field: SerializeField] public bool Paused { get; set; } = false;
        [field: SerializeField] public Transform Container { get; private set; }

        public Vector2 Center => Container ? Container.position : transform.position;
        
        public Vector2 DirectionDesire { get; private set; }
        public Vector2 DirectionActual
        {
            get => Container ? Container.up : transform.up;
            private set => Container.rotation = Quaternion.LookRotation(Vector3.forward, value);
        }

        public void Look(Vector2 direction)
        {
            if (direction.sqrMagnitude > 0f)
            {
                DirectionDesire = direction.normalized;
            }
        }
        
        private void InitContainer()
        {
            if (Container == null)
            {
                Container = new GameObject(nameof(Container)).transform;
                Container.SetParent(transform, false);
            }

            DirectionDesire = DirectionActual;
        }

        private void UpdateRotation(float deltaTime)
        {
            if (Paused) return;
            
            var deltaAngle = deltaTime * RotateSettings.angularSpeedMax;
            DirectionActual = Vector3.RotateTowards(DirectionActual, DirectionDesire, deltaAngle * Mathf.Deg2Rad, 0f);
        }
        
        private void Awake()
        {
            InitContainer();
        }

        private void Update()
        {
            UpdateRotation(Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.darkCyan;
            Gizmos.DrawRay(Center, DirectionDesire);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(Center, DirectionActual);
        }
    }

    [Serializable]
    public struct RotateSettings
    {
        public float angularSpeedMax;
        
        public static RotateSettings Default => new RotateSettings() { angularSpeedMax = 360f };
    }
}