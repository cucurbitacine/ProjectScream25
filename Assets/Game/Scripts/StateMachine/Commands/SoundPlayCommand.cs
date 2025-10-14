using System.Collections;
using Game.Scripts.Core;
using Game.Scripts.Sound;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Commands
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Commands/Sound Play Command", fileName = "SoundPlayCommand", order = 0)]
    public class SoundPlayCommand : CommandPreset
    {
        [field: Min(0f)]
        [field: SerializeField] public float MaxPlayFrequency { get; private set; } = 1;
        [field: Range(0f, 1f)]
        [field: SerializeField] public float Probability { get; private set; } = 1;
        [field: SerializeField] public SoundFxPreset SoundFx { get; private set; }

        public float MaxPlayCooldown => MaxPlayFrequency > 0f ? 1f / MaxPlayFrequency : float.MaxValue;
        
        public override ICommandExecutor CreateExecutor()
        {
            return new SoundPlayExecutor();
        }
    }

    public sealed class SoundPlayExecutor : CommandExecutor<SoundPlayCommand>
    {
        [InjectComponent] private SoundSourceReference soundSourceReference;

        private float lastPlayTime = float.MinValue;
        
        public override void Execute()
        {
            if (Preset.Probability <= 0f) return;
            
            if (Preset.Probability < 1f)
            {
                if (Preset.Probability < Random.value) return;
            }
            
            if (Time.time - lastPlayTime < Preset.MaxPlayCooldown)
            {
                return;
            }

            lastPlayTime = Time.time;
            
            if (Value.FloatValue > 0f)
            {
                soundSourceReference.StartCoroutine(DelayPlay(Value.FloatValue));
            }
            else
            {
                SoundPlay();
            }
            
            return;

            IEnumerator DelayPlay(float delay)
            {
                yield return new WaitForSeconds(delay);

                SoundPlay();
            }
        }

        private void SoundPlay()
        {
            if (soundSourceReference && soundSourceReference.SoundSource && Preset.SoundFx)
            {
                soundSourceReference.SoundSource.Play(Preset.SoundFx);
            }
        }
    }
}