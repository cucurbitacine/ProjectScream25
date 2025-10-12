using StateMachines.Data;
using UnityEngine;

namespace StateMachines.Presets
{
    public abstract class ValuePreset : ScriptableObject
    {
        [field: SerializeField] public string ValueName { get; private set; }
        
        public abstract ValueType ValueType { get; }
        
        public abstract IValueProvider CreateProvider();
        
        protected virtual void Reset()
        {
            ValueName = name;
        }
        
        protected virtual void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(ValueName))
            {
                ValueName = name;
            }
        }
    }

    public enum ValueType
    {
        Bool,
        Int,
        Float,
        String,
    }
    
    public abstract class BoolValuePreset : ValuePreset
    {
        public sealed override ValueType ValueType => ValueType.Bool;
    }
    
    public abstract class IntValuePreset : ValuePreset
    {
        public sealed override ValueType ValueType => ValueType.Int;
    }
    
    public abstract class FloatValuePreset : ValuePreset
    {
        public sealed override ValueType ValueType => ValueType.Float;
    }
}