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
        public IValueProvider ValueProvider;
        
        public bool IsTrue()
        {
            if (ValueType == ValueType.Bool)
            {
                if (Operation is OperatorType.Equals or OperatorType.LessOrEquals or OperatorType.GreaterOrEquals)
                {
                    return ValueProvider.GetBool() == ConditionValue.BoolValue;
                }
                
                return ValueProvider.GetBool() != ConditionValue.BoolValue;
            }
            
            if (ValueType == ValueType.Int)
            {
                if (Operation is OperatorType.NotEqual)
                {
                    return ValueProvider.GetInt() != ConditionValue.IntValue;
                }
                
                if (Operation is OperatorType.Less)
                {
                    return ValueProvider.GetInt() < ConditionValue.IntValue;
                }
                
                if (Operation is OperatorType.LessOrEquals)
                {
                    return ValueProvider.GetInt() <= ConditionValue.IntValue;
                }
                
                if (Operation is OperatorType.GreaterOrEquals)
                {
                    return ValueProvider.GetInt() >= ConditionValue.IntValue;
                }
                
                if (Operation is OperatorType.Greater)
                {
                    return ValueProvider.GetInt() > ConditionValue.IntValue;
                }
                
                return ValueProvider.GetInt() == ConditionValue.IntValue;
            }
            
            if (ValueType == ValueType.Float)
            {
                if (Operation is OperatorType.NotEqual)
                {
                    return !Mathf.Approximately(ValueProvider.GetFloat(), ConditionValue.FloatValue);
                }
                
                if (Operation is OperatorType.Less)
                {
                    return ValueProvider.GetFloat() < ConditionValue.FloatValue;
                }
                
                if (Operation is OperatorType.LessOrEquals)
                {
                    return ValueProvider.GetFloat() <= ConditionValue.FloatValue;
                }
                
                if (Operation is OperatorType.GreaterOrEquals)
                {
                    return ValueProvider.GetFloat() >= ConditionValue.FloatValue;
                }
                
                if (Operation is OperatorType.Greater)
                {
                    return ValueProvider.GetFloat() > ConditionValue.FloatValue;
                }
                
                return Mathf.Approximately(ValueProvider.GetFloat(), ConditionValue.FloatValue);
            }
            
            if (ValueType == ValueType.String)
            {
                if (Operation is OperatorType.Equals)
                {
                    return string.Equals(ValueProvider.GetString(), ConditionValue.StringValue);
                }
                
                if (Operation is OperatorType.NotEqual)
                {
                    return !string.Equals(ValueProvider.GetString(), ConditionValue.StringValue);
                }
            }
            
            return false;
        }
        
        public void Dispose()
        {
            ValueProvider?.Dispose();
        }
    }
}