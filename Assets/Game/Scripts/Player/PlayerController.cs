using System;
using Game.Scripts.Combat;
using Game.Scripts.Control;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Game.Scripts.Input.PlayerInput;

namespace Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cc;
        [Space]
        [SerializeField] private GunController gun;
        [field: SerializeField] public PlayerInput Input { get; private set; }

        private CinemachinePositionComposer cpc;
        
        public KinematicBody Kinematic { get; private set; }
        public VisualBody Visual { get; private set; }
        
        public Camera CameraMain => Camera.main;
        public Vector2 LookAtPoint => CameraMain.ScreenToWorldPoint(Mouse.current.position.value);

        private void OnAttacked()
        {
            gun.Fire();
        }
        
        private void OnTookAim(bool aim)
        {
            //
        }
        
        private void OnEnable()
        {
            Input.Attacked += OnAttacked;
            Input.TookAim += OnTookAim;
        }

        private void OnDisable()
        {
            Input.Attacked -= OnAttacked;
            Input.TookAim -= OnTookAim;
        }

        private void Awake()
        {
            Visual = GetComponent<VisualBody>();
            Kinematic = GetComponent<KinematicBody>();

            if (cc)
            {
                cpc = cc.GetComponent<CinemachinePositionComposer>();
            }
        }

        [SerializeField] private float lensSize = 2f;
        [SerializeField] private float lensSizeAim = 3f;
        [SerializeField] private Vector2 aimDistance = Vector2.zero;
        [SerializeField] private bool useDesire = false;
        
        private void LateUpdate()
        {
            if (cc)
            {
                var lensDesire = Input.Aim
                    ? lensSizeAim
                    : lensSize;
                
                cc.Lens.OrthographicSize = Mathf.Lerp(cc.Lens.OrthographicSize, lensDesire, Time.deltaTime);
            }
            
            if (cpc)
            {
                cpc.TargetOffset = Input.Aim
                    ? (useDesire ? Visual.DirectionDesire : Visual.DirectionActual) * aimDistance
                    : Vector2.zero;
            }
        }
    }
}