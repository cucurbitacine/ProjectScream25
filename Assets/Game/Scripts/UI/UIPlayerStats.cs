using Game.Scripts.Combat;
using Game.Scripts.Items;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class UIPlayerStats : MonoBehaviour
    {
        [SerializeField] private UIValueBar healthBar;
        [SerializeField] private UIValueBar lightBar;
        [SerializeField] private UIValueBar ammoBar;

        private HealthController _health;
        private FlashlightController _flashlight;
        private GunController _gun;
        
        public void Initialize(HealthController health, FlashlightController flashlight, GunController gun)
        {
            _health = health;
            _flashlight = flashlight;
            _gun = gun;

            OnHealthChanged(_health.Value, _health.Maximum);
            OnPowerChanged(_flashlight.Power, _flashlight.PowerCapacity);
            OnAmmoChanged(_gun.Ammo, _gun.AmmoCapacity, _gun.AmmoStorage);
            
            _health.HealthChanged += OnHealthChanged;
            _flashlight.PowerChanged += OnPowerChanged;
            _gun.AmmoChanged += OnAmmoChanged;
        }

        public void Deinitialize()
        {
            _health.HealthChanged -= OnHealthChanged;
            _flashlight.PowerChanged -= OnPowerChanged;
            _gun.AmmoChanged -= OnAmmoChanged;
            
            _health = null;
            _flashlight = null;
            _gun = null;
        }
        
        private void OnHealthChanged(int value, int maximum)
        {
            healthBar.SetValue(value, maximum);
        }
        
        private void OnPowerChanged(float value, float maximum)
        {
            lightBar.SetValue(value, maximum);
        }

        private void OnAmmoChanged(int value, int maximum, int storage)
        {
            ammoBar.SetValue(value, maximum);
        }
    }
}
