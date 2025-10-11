using System;
using UnityEngine;

namespace StateMachines.Presets
{
    [Serializable]
    public sealed class ConditionPreset
    {
        public ValuePreset ValuePreset;
        public OperatorType Operation = OperatorType.Equals;

        [Space]
        public bool BoolValue;
        public int IntValue;
        public float FloatValue;
        public string StringValue;
    }

    public enum OperatorType
    {
        NotEqual = 1,
        Less = 2,
        LessOrEquals = 4,
        Equals = 0,
        GreaterOrEquals = 5,
        Greater = 3,
    }
}