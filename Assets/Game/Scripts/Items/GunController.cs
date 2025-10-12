using System.Collections;
using Game.Scripts.Combat;
using Game.Scripts.Sound;
using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.Scripts.Items
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private DamagePreset damagePreset;

        [Space]
        [SerializeField] private Vector2 localOriginFireOffset = Vector2.zero; 
        [Min(0f)]
        [SerializeField] private float distanceFire = 100f;
        [SerializeField] private LayerMask targetLayer = 1;
        [SerializeField] private LayerMask obstacleLayer = 1;

        [Space]
        [Min(0f)]
        [SerializeField] private float frequencyFire = 1f;
        [Min(0)]
        [SerializeField] private int ammoPerShot = 1;
        [Min(0)]
        [SerializeField] private int ammo = 0;
        [Min(0)]
        [SerializeField] private int ammoCapacity = 12;
        [Min(0)]
        [SerializeField] private int ammoStorage = 12;
        [Min(0f)]
        [SerializeField] private float reloadDuration = 12;

        [Space]
        [SerializeField] private GameObject aim;
        
        [Header("VFX")]
        [Space]
        [SerializeField] private Light2DBase flashLight;
        [Min(0f)]
        [SerializeField] private float flashDuration = 0.1f;
        
        [Space]
        [SerializeField] private LineRenderer trailLine;
        [Min(0f)]
        [SerializeField] private float trailDuration = 0.1f;
        
        [Space]
        [SerializeField] private GameObject fireEffectPrefab;
        [SerializeField] private GameObject hitEffectPrefab;

        [Header("SFX")]
        [SerializeField] private SoundSource gunSfx;
        [SerializeField] private SoundFxPreset firePreset;
        [SerializeField] private SoundFxPreset emptyPreset;
        [SerializeField] private SoundFxPreset reloadBeginPreset;
        [SerializeField] private SoundFxPreset reloadMiddlePreset;
        [SerializeField] private SoundFxPreset reloadEndPreset;
        
        [Space]
        [SerializeField] private SoundSource hitSfx;
        [SerializeField] private SoundPack hitSoundPack;
        
        private Coroutine flashingCoroutine;
        private Coroutine trailingCoroutine;
        private Coroutine reloadingCoroutine;
        private float lastFireTime = float.MinValue;
        private bool reloading;
        
        private float PeriodFire => frequencyFire > 0f ? 1f / frequencyFire : float.MaxValue;

        public Vector2 OriginFire => transform.TransformPoint(localOriginFireOffset);
        public Vector2 DirectionFire => transform.up;
        public float DistanceFire => distanceFire;
        public LayerMask LayerFire => targetLayer | obstacleLayer;
        public RaycastHit2D HitFire { get; private set; }
        
        public void Fire()
        {
            if (reloading) return;
            
            var fireTime = Time.time;

            if (fireTime - lastFireTime < PeriodFire) return;
            
            lastFireTime = fireTime;

            if (ammo > 0)
            {
                ammo -= ammoPerShot;
                
                gunSfx.Play(firePreset);
            }
            else
            {
                gunSfx.Play(emptyPreset);
                return;
            }
            
            Flash();

            Trail();
                
            Hit();
        }
        
        public void Reload()
        {
            if (reloading) return;
            
            if (ammo >= ammoCapacity) return;
            if (ammoStorage <= 0) return;
            
            if (reloadingCoroutine != null) StopCoroutine(reloadingCoroutine);
            reloadingCoroutine = StartCoroutine(Reloading());
            
            return;
            
            IEnumerator Reloading()
            {
                reloading = true;
                
                gunSfx.Play(reloadBeginPreset);
                yield return new WaitForSeconds(reloadDuration * 0.5f);
                gunSfx.Play(reloadMiddlePreset);
                yield return new WaitForSeconds(reloadDuration * 0.5f);
                gunSfx.Play(reloadEndPreset);
                
                var deltaAmmo = Mathf.Min(ammoCapacity - ammo, ammoStorage);
                ammoStorage -= deltaAmmo;
                ammo += deltaAmmo;
                
                reloading = false;
            }
        }
        
        private void Flash()
        {
            if (flashingCoroutine != null) StopCoroutine(flashingCoroutine);
            flashingCoroutine = StartCoroutine(Flashing());
            
            return;
            
            IEnumerator Flashing()
            {
                flashLight.enabled = true;
                fireEffectPrefab.SetActive(true);
                yield return new WaitForSeconds(flashDuration);
                flashLight.enabled = false;
                fireEffectPrefab.SetActive(false);
            }
        }

        private void Trail()
        {
            if (trailingCoroutine != null) StopCoroutine(trailingCoroutine);
            trailingCoroutine = StartCoroutine(Trailing());
            
            return;
            
            IEnumerator Trailing()
            {
                var hitPoint = HitFire ? HitFire.point : (OriginFire + DirectionFire * DistanceFire);
                
                trailLine.SetPosition(0, OriginFire);
                trailLine.SetPosition(1, hitPoint);
                
                trailLine.enabled = true;
                yield return new WaitForSeconds(trailDuration);
                trailLine.enabled = false;
            }
        }
        
        private void Hit()
        {
            if (!HitFire) return;

            if (HitFire.collider.TryGetComponent(out SoundTypeSurface soundTypeHolder))
            {
                if (soundTypeHolder.SoundType && hitSoundPack.TryGetSoundFx(soundTypeHolder.SoundType, out var soundFx))
                {
                    hitSfx.Play(soundFx, HitFire.point);
                }
            }
            
            if (!targetLayer.ContainsMask(HitFire.collider.gameObject))
            {
                Debug.Log($"Hit Obstacle");
                return;
            }
            
            Debug.Log($"Hit Target");

            if (HitFire.collider.TryGetComponent<Hitbox>(out var hitbox))
            {
                Debug.Log($"Hit Hitbox");
                
                hitbox.Damage(damagePreset.Amount);
            }
        }
        
        private void Awake()
        {
            flashLight.enabled = false;
            trailLine.enabled = false;
        }

        private void Update()
        {
            if (aim)
            {
                if (HitFire)
                {
                    aim.transform.position = HitFire.point;
                    aim.transform.rotation = Quaternion.LookRotation(Vector3.forward, HitFire.normal);
                    
                    aim.SetActive(true);
                }
                else
                {
                    aim.SetActive(false);
                }
            }
        }

        private void FixedUpdate()
        {
            HitFire = Physics2D.Raycast(OriginFire, DirectionFire, DistanceFire, LayerFire);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(OriginFire, 0.1f);
            
            if (HitFire)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(OriginFire, HitFire.point);
                Gizmos.DrawWireSphere(HitFire.point, 0.1f);
            }
            else
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawRay(OriginFire, DirectionFire * DistanceFire);
            }
        }
    }
}
