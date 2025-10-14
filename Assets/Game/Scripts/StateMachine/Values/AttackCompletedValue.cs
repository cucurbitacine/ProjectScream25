using Game.Scripts.StateMachine.Features;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Values
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Values/Attack Completed Value", fileName = "AttackCompletedValue", order = 0)]
    public class AttackCompletedValue : BoolValuePreset
    {
        public override IValueProvider CreateProvider()
        {
            return new AttackCompletedValueProvider();
        }
    }

    public sealed class AttackCompletedValueProvider : ValueProvider<AttackCompletedValue>
    {
        [InjectProcess] private AttackProcess attackProcess;
        
        public override bool GetBool()
        {
            return attackProcess.IsAttackCompleted;
        }
    }
}