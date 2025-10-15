using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Sound
{
    public class SoundTypeSurface : MonoBehaviour
    {
        [field: SerializeField] public ObjectType SoundType { get; private set; }
    }
}