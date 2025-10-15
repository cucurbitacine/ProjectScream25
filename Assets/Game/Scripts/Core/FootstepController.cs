using System;
using Game.Scripts.Control;
using Game.Scripts.Sound;
using UnityEngine;

namespace Game.Scripts.Core
{
    [RequireComponent(typeof(KinematicBody))]
    public class FootstepController : MonoBehaviour
    {
        [Header("Step")]
        [SerializeField] private AnimationCurve stepFrequencyPerSpeed = AnimationCurve.Constant(0, 1, 2);
        
        [Header("Sound")]
        [SerializeField] private SoundSource soundSource;
        [SerializeField] private SoundPack soundPack;
        [SerializeField] private SoundFxPreset soundFxDefault;

        private float lastStepTime = float.MinValue;
        private KinematicBody kinematic;

        private float Speed => kinematic ? kinematic.VelocityActual.magnitude : 0f;
        private float StepFrequency => stepFrequencyPerSpeed.Evaluate(Speed);
        private float StepCooldown => StepFrequency > 0f ? 1f / StepFrequency : float.MaxValue;
        
        private void Step()
        {
            var soundFx = soundFxDefault;
            
            if (TryGetSurfaceType(out var surfaceType))
            {
                if (soundPack.TryGetSoundFx(surfaceType, out var surfaceSoundFx))
                {
                    soundFx = surfaceSoundFx;
                }
            }
            
            soundSource.Play(soundFx);
        }

        private bool TryGetSurfaceType(out ObjectType surfaceType)
        {
            surfaceType = null;
            return false;
        }

        private void Awake()
        {
            kinematic = GetComponent<KinematicBody>();
        }

        private void Update()
        {
            if (kinematic.VelocityActual.sqrMagnitude <= 0.1f) return;

            if (Time.time - lastStepTime > StepCooldown)
            {
                Step();

                lastStepTime = Time.time;
            }
        }
    }
}
