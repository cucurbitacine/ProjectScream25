using System;
using StateMachines.Presets;
using UnityEngine;
using ValueType = StateMachines.Presets.ValueType;

namespace StateMachines.Data
{
    public sealed class ConditionData : IDisposable
    {
        public StateMachineData StateMachine;
        public int StateId;
        public ValueType ValueType;
        public Value ConditionValue;
        public OperatorType Operation;
        public IValueData ValueData;
        
        public bool IsTrue()
        {
            if (ValueType == ValueType.Bool)
            {
                if (Operation is OperatorType.Equals or OperatorType.LessOrEquals or OperatorType.GreaterOrEquals)
                {
                    return ValueData.GetBool() == ConditionValue.BoolValue;
                }
                
                return ValueData.GetBool() != ConditionValue.BoolValue;
            }
            
            if (ValueType == ValueType.Int)
            {
                if (Operation is OperatorType.NotEqual)
                {
                    return ValueData.GetInt() != ConditionValue.IntValue;
                }
                
                if (Operation is OperatorType.Less)
                {
                    return ValueData.GetInt() < ConditionValue.IntValue;
                }
                
                if (Operation is OperatorType.LessOrEquals)
                {
                    return ValueData.GetInt() <= ConditionValue.IntValue;
                }
                
                if (Operation is OperatorType.GreaterOrEquals)
                {
                    return ValueData.GetInt() >= ConditionValue.IntValue;
                }
                
                if (Operation is OperatorType.Greater)
                {
                    return ValueData.GetInt() > ConditionValue.IntValue;
                }
                
                return ValueData.GetInt() == ConditionValue.IntValue;
            }
            
            if (ValueType == ValueType.Float)
            {
                if (Operation is OperatorType.NotEqual)
                {
                    return !Mathf.Approximately(ValueData.GetFloat(), ConditionValue.FloatValue);
                }
                
                if (Operation is OperatorType.Less)
                {
                    return ValueData.GetFloat() < ConditionValue.FloatValue;
                }
                
                if (Operation is OperatorType.LessOrEquals)
                {
                    return ValueData.GetFloat() <= ConditionValue.FloatValue;
                }
                
                if (Operation is OperatorType.GreaterOrEquals)
                {
                    return ValueData.GetFloat() >= ConditionValue.FloatValue;
                }
                
                if (Operation is OperatorType.Greater)
                {
                    return ValueData.GetFloat() > ConditionValue.FloatValue;
                }
                
                return Mathf.Approximately(ValueData.GetFloat(), ConditionValue.FloatValue);
            }
            
            if (ValueType == ValueType.String)
            {
                if (Operation is OperatorType.Equals)
                {
                    return string.Equals(ValueData.GetString(), ConditionValue.StringValue);
                }
                
                if (Operation is OperatorType.NotEqual)
                {
                    return !string.Equals(ValueData.GetString(), ConditionValue.StringValue);
                }
            }
            
            return false;
        }
        
        public void Dispose()
        {
            ValueData?.Dispose();
        }
    }
}