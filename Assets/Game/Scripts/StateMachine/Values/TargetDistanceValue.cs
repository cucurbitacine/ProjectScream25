using Game.Scripts.Combat;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Values
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Values/Target Distance Value", fileName = "TargetDistanceValue", order = 0)]
    public class TargetDistanceValue : FloatValuePreset
    {
        public override IValueProvider CreateProvider()
        {
            return new TargetDistanceValueProvider();
        }
    }

    public sealed class TargetDistanceValueProvider : ValueProvider<TargetDistanceValue>
    {
        [InjectComponent] private TargetSelectController targetSelect;
        
        public override float GetFloat()
        {
            if (targetSelect == null || !targetSelect.HasTarget) return float.MaxValue;

            return Vector2.Distance(targetSelect.OriginPosition, targetSelect.Target.transform.position);
        }
    }
}