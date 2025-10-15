using System;
using UnityEngine;

namespace Game.Scripts.Combat
{
    public class HealthController : MonoBehaviour
    {
        [field: Min(0)]
        [field: SerializeField] public int Value { get; private set; } = 100;
        [field: Min(1)]
        [field: SerializeField] public int Maximum { get; private set; } = 100;

        public event Action<int, int> HealthChanged; 
        public event Action<bool> Died; 
        
        public bool IsDead => Value == 0;
        
        public void SetMaximum(int maximum)
        {
            var oldMaximum = Maximum;
            Maximum = Mathf.Max(1, maximum);

            if (oldMaximum != Maximum)
            {
                HealthChanged?.Invoke(Value, Maximum);
            }
        }
        
        public void SetHealth(int value)
        {
            var oldValue = Value;
            Value = Mathf.Clamp(value, 0, Maximum);

            if (oldValue != Value)
            {
                HealthChanged?.Invoke(Value, Maximum);
                
                if (oldValue == 0 && Value > 0)
                {
                    Died?.Invoke(false);
                }
                else if (oldValue > 0 && Value == 0)
                {
                    Died?.Invoke(true);
                }
            }
        }
        
        public void SetHealth(int value, int maximum)
        {
            SetMaximum(maximum);
            
            SetHealth(value);
        }
        
        public void Damage(int amount)
        {
            if (IsDead) return;
            if (amount <= 0) return;
            
            SetHealth(Value - amount);
        }
        
        public void Heal(int amount)
        {
            if (IsDead) return;
            if (amount <= 0) return;
            
            SetHealth(Value + amount);
        }

        public void Resurrect(int health)
        {
            if (!IsDead) return;
            health = Mathf.Max(1, health);
            
            SetHealth(health);
        }

        private void OnValidate()
        {
            if (Value > Maximum)
            {
                Value = Maximum;
            }
        }
    }
}