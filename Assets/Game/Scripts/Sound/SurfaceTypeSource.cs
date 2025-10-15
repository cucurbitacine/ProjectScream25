using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Sound
{
    public class SurfaceTypeSource : MonoBehaviour
    {
        [field: SerializeField] public ObjectType SurfaceType { get; private set; }
    }
}