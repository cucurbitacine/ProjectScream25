using UnityEngine;

namespace Game.Scripts.Combat
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Combat/Damage Preset", fileName = "DamagePreset", order = 0)]
    public class DamagePreset : ScriptableObject
    {
        [Min(1)]
        public int Amount = 1;
    }
}