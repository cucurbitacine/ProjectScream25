using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class LookAtPoint : MonoBehaviour
    {
        [SerializeField] private bool clampAngle = true;
        [Range(-180f, 180f)]
        [SerializeField] private float angleOffset = 0f;
        [Range(0f, 360f)]
        [SerializeField] private float angleSector = 360;
        [Space]
        [SerializeField] private PlayerController player;

        private Vector2 center => transform.position;

        private Quaternion rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        private Vector2 direction
        {
            get => transform.up;
            set
            {
                if (clampAngle)
                {
                    var signedAngle = Vector2.SignedAngle(centerDirection, value);

                    signedAngle = Mathf.Clamp(signedAngle, -angleSector * 0.5f, angleSector * 0.5f);

                    rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0f, 0f, signedAngle) * centerDirection);
                }
                else
                {
                    rotation = Quaternion.LookRotation(Vector3.forward, value);
                }
            }
        }

        private Transform parent => transform.parent ? transform.parent : transform;

        private Vector2 centerDirection => Quaternion.Euler(0f, 0f, -angleOffset) * parent.up;

        private void Update()
        {
            if (player.IsDead) return;
            
            var deltaAngle = player.Visual.RotateSettings.angularSpeedMax * Time.deltaTime;
            direction = Vector3.RotateTowards(direction, player.LookAtPoint - center, deltaAngle * Mathf.Deg2Rad, 0f);
            //direction = player.LookAtPoint - center;
        }

        private void OnDrawGizmos()
        {
            if (clampAngle)
            {
                Gizmos.DrawRay(center, centerDirection);
                if (angleSector < 360f)
                {
                    Gizmos.DrawRay(center, Quaternion.Euler(0f, 0f, -angleSector * 0.5f) * centerDirection);
                    Gizmos.DrawRay(center, Quaternion.Euler(0f, 0f, angleSector * 0.5f) * centerDirection);
                }
            }
        }
    }
}