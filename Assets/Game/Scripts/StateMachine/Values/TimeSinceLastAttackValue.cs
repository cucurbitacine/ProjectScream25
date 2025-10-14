using Game.Scripts.Combat;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Values
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Values/Time Since Last Attack Value", fileName = "TimeSinceLastAttackValue", order = 0)]
    public class TimeSinceLastAttackValue : FloatValuePreset
    {
        public override IValueProvider CreateProvider()
        {
            return new TimeSinceLastAttackValueProvider();
        }
    }
    
    public sealed class TimeSinceLastAttackValueProvider : ValueProvider<TimeSinceLastAttackValue>
    {
        [InjectComponent] private AttackController attacker;
        
        public override float GetFloat()
        {
            return Time.time - attacker.LastAttackTime;
        }
    }
}