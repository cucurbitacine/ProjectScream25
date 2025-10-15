using System;
using Game.Scripts.Combat;
using Game.Scripts.Items;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] private UIPlayerStats playerStats;

        private PlayerController _player;
        
        public void Initialize(PlayerController player)
        {
            _player = player;

            var health = _player.GetComponent<HealthController>();
            var flashlight = _player.GetComponentInChildren<FlashlightController>();
            var gun = _player.GetComponentInChildren<GunController>();
            
            playerStats.Initialize(health, flashlight, gun);
        }

        public void Deinitialize()
        {
            playerStats.Deinitialize();
        }
    }
}
