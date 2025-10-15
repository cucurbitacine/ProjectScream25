using Game.Scripts.Core;
using Game.Scripts.Sound;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Commands
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Commands/Sound Stop Command", fileName = "SoundStopCommand", order = 0)]
    public class SoundStopCommand : CommandPreset
    {
        public override ICommandExecutor CreateExecutor()
        {
            return new SoundStopExecutor();
        }
    }

    public sealed class SoundStopExecutor : CommandExecutor<SoundStopCommand>
    {
        [InjectComponent] private SoundSourceReference soundSourceReference; 
        
        public override void Execute()
        {
            if (soundSourceReference && soundSourceReference.SoundSource)
            {
                soundSourceReference.SoundSource.Stop();
            }
        }
    }
}