using UnityEngine;

namespace Game.Scripts.Sound
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Sound/Sound Type", fileName = "SoundType", order = 0)]
    public class SoundType : ScriptableObject
    {
        [SerializeField] private string description;
    }
}