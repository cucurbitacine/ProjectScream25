using Game.Scripts.Combat;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Values
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Values/Has Target Value", fileName = "HasTargetValue", order = 0)]
    public class HasTargetValue : BoolValuePreset
    {
        public override IValueProvider CreateProvider()
        {
            return new HasTargetValueProvider();
        }
    }

    public sealed class HasTargetValueProvider : ValueProvider<HasTargetValue>
    {
        [InjectComponent] private TargetSelectController targetSelect;
        
        public override bool GetBool()
        {
            return targetSelect.HasTarget;
        }
    }
}