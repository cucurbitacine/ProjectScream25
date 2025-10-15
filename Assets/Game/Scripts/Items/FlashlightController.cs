using System;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.Scripts.Items
{
    public class FlashlightController : MonoBehaviour
    {
        [Min(0f)]
        [field: SerializeField] public float Power { get; private set; } = 100;
        [Min(0f)]
        [field: SerializeField] public float PowerCapacity { get; private set; } = 100;
        
        [field: Space]
        [Min(0f)]
        [field: SerializeField] public float PowerConsumption { get; private set; } = 10;
        [Min(0f)]
        [field: SerializeField] public float PowerRecovery { get; private set; } = 10;

        [Space]
        [SerializeField] private bool turnedOnByDefault = true;
        [Min(0f)]
        [SerializeField] private float cooldownAfterBlackout = 3f;
        [SerializeField] private Light2DBase flashlight;

        private float lastBlackout = float.MinValue;
        
        public event Action<bool> TurnedOn; 
        public event Action<float, float> PowerChanged; 
        
        public bool IsCooldown => Time.time - lastBlackout < cooldownAfterBlackout;
        public bool BlackoutStart => Power <= 0f;
        public bool StatusActual
        {
            get => flashlight.enabled;
            private set => flashlight.enabled = value;
        }

        public void Turn(bool value, bool forced)
        {
            if (!forced && StatusActual == value) return;

            if (value && BlackoutStart) return;
            if (value && IsCooldown) return;
            
            StatusActual = value;
            
            TurnedOn?.Invoke(StatusActual);
        }
        
        public void Turn(bool value)
        {
            Turn(value, false);
        }
        
        public void TurnOn()
        {
            Turn(true);
        }
        
        public void TurnOff()
        {
            Turn(false);
        }
        
        private void Start()
        {
            Turn(turnedOnByDefault, true);
        }

        private void Update()
        {
            if (StatusActual)
            {
                if (BlackoutStart)
                {
                    TurnOff();

                    lastBlackout = Time.time;
                }
                else
                {
                    var oldPower = Power;
                    Power = Mathf.Clamp(Power - PowerConsumption * Time.deltaTime, 0, PowerCapacity);
                    if (!Mathf.Approximately(oldPower, Power))
                    {
                        PowerChanged?.Invoke(Power, PowerCapacity);
                    }
                }
            }
            else
            {
                var oldPower = Power;
                Power = Mathf.Clamp(Power + PowerRecovery * Time.deltaTime, 0, PowerCapacity);
                if (!Mathf.Approximately(oldPower, Power))
                {
                    PowerChanged?.Invoke(Power, PowerCapacity);
                }
            }
        }
    }
}
