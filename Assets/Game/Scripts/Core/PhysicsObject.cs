using UnityEngine;

namespace Game.Scripts.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsObject : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField] private float linearDamping = 20;
        [Min(0f)]
        [SerializeField] private float angularDamping = 50;
        
        private Rigidbody2D rgb;
        
        private void InitRigidbody()
        {
            rgb = GetComponent<Rigidbody2D>();
            
            rgb.gravityScale = 0f;
            rgb.linearDamping = linearDamping;
            rgb.angularDamping = angularDamping;
                
            rgb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rgb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void Awake()
        {
            InitRigidbody();
        }
    }
}
