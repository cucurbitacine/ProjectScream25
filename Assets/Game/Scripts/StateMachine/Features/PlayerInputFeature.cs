using Game.Scripts.Player;
using Game.Scripts.Utils;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Features
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Features/Player Input Feature", fileName = nameof(PlayerInputFeature), order = 0)]
    public class PlayerInputFeature : FeaturePreset
    {
        [Min(0f)]
        public float MinDistanceToPoint = 0.1f;
        
        public override IFeatureProcess CreateProcess()
        {
            return new PlayerInputProcess();
        }
    }

    public sealed class PlayerInputProcess : FeatureProcess<PlayerInputFeature>
    {
        [InjectProcess] private MovementProcess movement;
        [InjectProcess] private RotationProcess rotation;
        [InjectComponent] private PlayerController player;

        public override void Execute(float deltaTime)
        {
            base.Execute(deltaTime);

            if (movement != null)
            {
                movement.Move(player.Input.Move);
            }
            
            if (rotation != null)
            {
                rotation.LookAt(player.LookAtPoint, Preset.MinDistanceToPoint);
            }
        }
    }
}