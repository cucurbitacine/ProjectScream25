using StateMachines.Data;
using StateMachines.Presets;

namespace StateMachines.Utils
{
    public static class ConditionUtils
    {
        public static ConditionData Bake(this ConditionPreset conditionPreset, StateMachineData stateMachine, int stateId)
        {
            var conditionData = new ConditionData();
            
            conditionData.StateMachine = stateMachine;
            conditionData.StateId = stateId;
            conditionData.ValueType = conditionPreset.ValuePreset.ValueType;
            conditionData.ConditionValue = new Value()
            {
                BoolValue = conditionPreset.BoolValue,
                IntValue = conditionPreset.IntValue,
                FloatValue = conditionPreset.FloatValue,
                StringValue = conditionPreset.StringValue,
            };
            conditionData.Operation = conditionPreset.Operation;
            conditionData.ValueData = conditionPreset.ValuePreset.Bake(conditionData.StateMachine, conditionData.StateId);
            
            return conditionData;
        }

        public static IValueData Bake(this ValuePreset valuePreset, StateMachineData stateMachine, int stateId)
        {
            var valueProvider = valuePreset.CreateProvider();
            valueProvider.Bake(stateMachine, stateId, valuePreset);
            return valueProvider;
        }
        
        public static void Initialize(this ConditionData conditionData)
        {
            conditionData.ValueData.Initialize();
        }
    }
}