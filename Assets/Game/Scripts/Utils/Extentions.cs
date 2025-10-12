using System.Linq;
using Game.Scripts.Control;
using Game.Scripts.Items;
using Game.Scripts.Sound;
using Game.Scripts.StateMachine.Features;
using UnityEngine;

namespace Game.Scripts.Utils
{
    public static class Extentions
    {
        public static bool ContainsMask(this int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;
        public static bool ContainsMask(this GameObject gameObject, LayerMask mask) => gameObject.layer.ContainsMask(mask);
        public static bool ContainsMask(this LayerMask mask, int layer) => layer.ContainsMask(mask);
        public static bool ContainsMask(this LayerMask mask, GameObject gameObject) => mask.ContainsMask(gameObject.layer);
        
        public static void LookAt(this VisualBody visual, Vector2 point, float minDistance = 0.0f)
        {
            var vector = point - visual.Center;
            if (vector.magnitude > minDistance)
            {
                visual.Look(vector);
            }
        }
        
        public static void LookAt(this RotationProcess rotation, Vector2 point, float minDistance = 0.0f)
        {
            var vector = point - rotation.Center;
            if (vector.magnitude > minDistance)
            {
                rotation.Look(vector);
            }
        }

        public static void Stop(this KinematicBody kinematic)
        {
            kinematic.Move(Vector2.zero);
        }

        public static void Switch(this FlashlightController flashlight)
        {
            flashlight.Turn(!flashlight.StatusActual);
        }
        
        // TODO it's very stupid - fix it later
        public static void Play(this SoundSource soundSource, Vector2 point)
        {
            soundSource.transform.position = point;
            soundSource.Play();
        }
        
        public static void Play(this SoundSource soundSource, SoundFxPreset soundFx, Vector2 point)
        {
            soundSource.transform.position = point;
            soundSource.Play(soundFx);
        }
    }
}