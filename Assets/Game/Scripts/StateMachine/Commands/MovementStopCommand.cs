using Game.Scripts.Control;
using Game.Scripts.Utils;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Commands
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Commands/Movement Stop Command", fileName = "MovementStopCommand", order = 0)]
    public class MovementStopCommand : CommandPreset
    {
        public MoveSettings MoveSettings = MoveSettings.Default;
        
        public override ICommandExecutor CreateExecutor()
        {
            return new StopExecutor();
        }
    }

    public sealed class StopExecutor : CommandExecutor<MovementStopCommand>
    {
        [InjectComponent] private KinematicBody kinematic;
        
        public override void Execute()
        {
            kinematic.MoveSettings = Preset.MoveSettings;
            
            kinematic.Stop();
        }
    }
}
