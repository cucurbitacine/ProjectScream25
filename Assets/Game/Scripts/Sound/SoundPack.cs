using System;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Sound
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Sound/Sound Pack", fileName = "Sound Pack", order = 0)]
    public class SoundPack : ScriptableObject
    {
        [SerializeField] private Pack[] packs;
        
        [Serializable]
        public struct Pack
        {
            public SoundType SoundType;
            public SoundFxPreset SoundFx;
        }

        public bool TryGetSoundFx(SoundType soundType, out SoundFxPreset soundFx)
        {
            return soundFx = packs.FirstOrDefault(s => s.SoundType == soundType).SoundFx;
        }
    }
}