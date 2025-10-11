using StateMachines.Data;
using StateMachines.Presets;
using UnityEngine;

namespace StateMachines.Impl
{
    [CreateAssetMenu(menuName = StateMachinePreset.CreateValueMenu + "/Time In State", fileName = "Time In State", order = 0)]
    public class TimeInState : FloatValuePreset
    {
        public override IValueData CreateProvider()
        {
            return new TimeInStateValue();
        }
    }

    public sealed class TimeInStateValue : ValueData<TimeInState>
    {
        public override float GetFloat()
        {
            return StateMachine.TimeInState;
        }
    }
}