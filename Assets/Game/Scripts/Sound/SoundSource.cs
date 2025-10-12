using UnityEngine;

namespace Game.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundSource : MonoBehaviour
    {
        [field: SerializeField] public SoundFxPreset SoundFx { get; private set; }
        public AudioSource AudioSource { get; private set; }

        public void Play()
        {
            if (AudioSource.isPlaying) AudioSource.Stop();
            
            AudioSource.volume = SoundFx.Volume;
            AudioSource.loop = SoundFx.Looped;
            AudioSource.outputAudioMixerGroup = SoundFx.AudioMixerGroup;
            AudioSource.spatialBlend = SoundFx.Is3D ? 1f : 0f;

            if (SoundFx.PlayOneShot)
            {
                AudioSource.PlayOneShot(SoundFx.GetAudioClip(), SoundFx.Volume);
            }
            else
            {
                AudioSource.resource = SoundFx.GetAudioClip();
                AudioSource.Play();
            }
        }
        
        public void Play(SoundFxPreset soundFx)
        {
            SoundFx = soundFx;
            
            Play();
        }
        
        private void InitAudio()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.playOnAwake = false; 
            
            if (SoundFx && SoundFx.PlayOnEnable)
            {
                AudioSource.playOnAwake = false;
                AudioSource.clip = SoundFx.GetAudioClip();
            }
        }
        
        private void Awake()
        {
            InitAudio();
        }

        private void OnEnable()
        {
            if (SoundFx && SoundFx.PlayOnEnable) Play();
        }
    }
}