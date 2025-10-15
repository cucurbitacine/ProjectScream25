using Game.Scripts.Combat;
using Game.Scripts.Control;
using Game.Scripts.Items;
using Game.Scripts.Utils;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Game.Scripts.Input.PlayerInput;

namespace Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private CinemachineCamera cc;
        [SerializeField] private float lensSize = 2f;
        [SerializeField] private float lensSizeAim = 3f;
        [SerializeField] private Vector2 aimDistance = Vector2.zero;
        [Min(0f)]
        [SerializeField] private float cameraDamping = 1f;
        
        [Header("Components")]
        [SerializeField] private GunController gun;
        [SerializeField] private FlashlightController flashlight;
        
        [field: Header("Input")]
        [field: SerializeField] public PlayerInput Input { get; private set; }

        private TargetNotifier targetNotifier;
        private CinemachinePositionComposer cpc;
        
        public KinematicBody Kinematic { get; private set; }
        public VisualBody Visual { get; private set; }
        public Damageable Damageable { get; private set; }
        
        public Camera CameraMain => Camera.main;
        public Vector2 LookAtPoint => CameraMain.ScreenToWorldPoint(Mouse.current.position.value);

        private void OnAttacked()
        {
            gun.Fire();
        }
        
        private void OnInteracted()
        {
            flashlight.Switch();
        }
        
        private void OnReloaded()
        {
            gun.Reload();
        }

        private void OnGunFired(bool fired)
        {
            if (fired)
            {
                targetNotifier.Notify();
            }
        }
        
        private void InitPlayer()
        {
            Visual = GetComponent<VisualBody>();
            Kinematic = GetComponent<KinematicBody>();
            Damageable = GetComponent<Damageable>();
            
            if (cc)
            {
                cpc = cc.GetComponent<CinemachinePositionComposer>();
            }

            targetNotifier = GetComponent<TargetNotifier>();
        }
        
        private void UpdateCamera(float deltaTime)
        {
            if (cc)
            {
                var lensDesire = Input.Aim
                    ? lensSizeAim
                    : lensSize;
                
                cc.Lens.OrthographicSize = Mathf.Lerp(cc.Lens.OrthographicSize, lensDesire, cameraDamping * deltaTime);
            }

            if (cpc)
            {
                var targetOffsetDesire = Input.Aim
                    //? (stopRotateDuringAiming ? (LookAtPoint - Visual.Center).normalized : Visual.DirectionActual) * aimDistance
                    ? ((gun.HitFire ? gun.HitFire.point : (gun.OriginFire + gun.DirectionFire * gun.DistanceFire)) - (Vector2)cpc.FollowTarget.position).normalized * aimDistance
                    //? ((gun.HitFire ? gun.HitFire.point : (gun.OriginFire + gun.DirectionFire * gun.DistanceFire)) - Visual.Center).normalized * aimDistance
                    : Vector2.zero;
                
                cpc.TargetOffset = Vector2.Lerp(cpc.TargetOffset, targetOffsetDesire, cameraDamping * deltaTime);
            }
        }

        private void Awake()
        {
            InitPlayer();
        }

        private void OnEnable()
        {
            Input.Attacked += OnAttacked;
            Input.Interacted += OnInteracted;
            Input.Reloaded += OnReloaded;
            gun.Fired += OnGunFired;
        }

        private void OnDisable()
        {
            Input.Attacked -= OnAttacked;
            Input.Interacted -= OnInteracted;
            Input.Reloaded -= OnReloaded;
            gun.Fired -= OnGunFired;
        }

        private void LateUpdate()
        {
            UpdateCamera(Time.deltaTime);
        }
    }
}