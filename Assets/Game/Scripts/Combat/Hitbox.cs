using System;
using UnityEngine;

namespace Game.Scripts.Combat
{
    public class Hitbox : MonoBehaviour
    {
        public bool Mute = false;
        public int Team = 0;
        
        public event Action<int> Damaged;

        public void Damage(int amount)
        {
            if (Mute) return;
            
            Damaged?.Invoke(amount);
        }
    }
}
