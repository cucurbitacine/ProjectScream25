using Game.Scripts.Control;
using Game.Scripts.Features;
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
    }
}