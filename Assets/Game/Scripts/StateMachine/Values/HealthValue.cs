using Game.Scripts.Combat;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Values
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Values/Health Value", fileName = "HealthValue", order = 0)]
    public class HealthValue : IntValuePreset
    {
        public override IValueProvider CreateProvider()
        {
            return new HealthValueProvider();
        }
    }

    public sealed class HealthValueProvider : ValueProvider<HealthValue>
    {
        [InjectComponent] private HealthController health;
        
        public override int GetInt()
        {
            return health.Value;
        }
    }
}