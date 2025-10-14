using Game.Scripts.Sound;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class SoundSourceReference : MonoBehaviour
    {
        [field: SerializeField] public SoundSource SoundSource { get; private set; } 
    }
}