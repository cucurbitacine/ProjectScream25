using System;
using UnityEngine;

namespace Game.Scripts.Control
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class KinematicBody : MonoBehaviour
    {
        public MoveSettings MoveSettings = MoveSettings.Default;
        
        public Vector2 Position => rgb ? rgb.position : transform.position;
        
        public Vector2 MoveDirection { get; private set; }
        public Vector2 VelocityDesire => MoveDirection * MoveSettings.speedMax;
        public Vector2 VelocityActual
        {
            get => rgb ? rgb.linearVelocity : VelocityDesire;
            private set => rgb.linearVelocity = value;
        }

        private Rigidbody2D rgb;
        private CircleCollider2D cld; // TODO

        public void Move(Vector2 move)
        {
            MoveDirection = move.sqrMagnitude > 1f ? move.normalized : move;
        }
        
        private void InitRigidbody()
        {
            rgb = GetComponent<Rigidbody2D>();
            
            rgb.gravityScale = 0f;
            rgb.linearDamping = 0f;
            rgb.angularDamping = 0f;
            
            rgb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rgb.interpolation = RigidbodyInterpolation2D.Interpolate;

            rgb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        private void InitCollider()
        {
            cld = GetComponent<CircleCollider2D>();
        }

        private void UpdateVelocity(float deltaTime)
        {
            VelocityActual = Vector2.MoveTowards(VelocityActual, VelocityDesire, MoveSettings.acceleration * deltaTime);
        }
        
        private void Awake()
        {
            InitRigidbody();
            InitCollider();
        }

        private void FixedUpdate()
        {
            UpdateVelocity(Time.fixedDeltaTime);
        }
        
        private void OnDrawGizmos()
        {
            if (MoveSettings.speedMax > 0f)
            {
                Gizmos.color = Color.darkGreen;
                Gizmos.DrawRay(Position, VelocityDesire / MoveSettings.speedMax);

                Gizmos.color = Color.green;
                Gizmos.DrawRay(Position, VelocityActual / MoveSettings.speedMax);
            }
        }
    }

    [Serializable]
    public struct MoveSettings
    {
        public float acceleration;
        public float speedMax;

        public static MoveSettings Default = new MoveSettings() { acceleration = 10f, speedMax = 5f };
    }
}
