using StateMachines.Data;
using StateMachines.Presets;
using UnityEngine;

namespace StateMachines.Impl
{
    [CreateAssetMenu(menuName = StateMachinePreset.CreateValueMenu + "/Raw Data", fileName = "Raw Data", order = 0)]
    public class RawValue : ValuePreset
    {
        [SerializeField] private ValueType _valueType;
        [field: SerializeField] public Value Value { get; private set; }
        
        public override ValueType ValueType => _valueType;
        
        public override IValueProvider CreateProvider()
        {
            return new RawValueProvider();
        }
    }

    public sealed class RawValueProvider : ValueProvider<RawValue>
    {
        public override bool GetBool()
        {
            return Preset.Value.BoolValue;
        }

        public override int GetInt()
        {
            return Preset.Value.IntValue;
        }

        public override float GetFloat()
        {
            return Preset.Value.FloatValue;
        }
    }
}