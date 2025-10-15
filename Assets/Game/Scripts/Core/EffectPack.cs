using System;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Core
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Effect Pack", fileName = "EffectPack", order = 0)]
    public class EffectPack : ScriptableObject
    {
        [SerializeField] private Pack[] packs;
        
        [Serializable]
        public struct Pack
        {
            public ObjectType EffectType;
            public GameObject Effect;
        }

        public bool TryGetEffect(ObjectType effectType, out GameObject effect)
        {
            return effect = packs.FirstOrDefault(s => s.EffectType == effectType).Effect;
        }
    }
}
