using UnityEngine;
using UnityEngine.Audio;

namespace Game.Scripts.Sound
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Sound/SoundFx Preset", fileName = "SoundFxPreset", order = 0)]
    public class SoundFxPreset : ScriptableObject
    {
        [field: SerializeField] public bool PlayOnEnable { get; private set; } = false;
        [field: SerializeField] public bool PlayOneShot { get; private set; } = false;
        [field: SerializeField] public bool Looped { get; private set; } = false;
        [field: SerializeField] public bool Is3D { get; private set; } = false;
        [field: SerializeField] public AudioMixerGroup AudioMixerGroup { get; private set; }
        
        [field: Range(0f, 1f)]
        [field: SerializeField] public float Volume { get; private set; } = 1f;
        
        [SerializeField] private AudioClip[] clips;

        public AudioClip GetAudioClip()
        {
            return clips[Random.Range(0, clips.Length)];
        }
    }
}