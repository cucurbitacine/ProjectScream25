using Game.Scripts.Control;
using Game.Scripts.Player;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Features
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Features/Rotation Feature", fileName = nameof(RotationFeature), order = 0)]
    public class RotationFeature : FeaturePreset
    {
        public RotateSettings RotateSettings = RotateSettings.Default;
        
        public override IFeatureProcess CreateProcess()
        {
            return new RotationProcess();
        }
    }

    public sealed class RotationProcess : FeatureProcess<RotationFeature>
    {
        [InjectComponent] private VisualBody visual;
        [InjectComponent] private PlayerController player;

        public Vector2 Center => visual.Center;
        
        public override void Enter()
        {
            base.Enter();
            
            visual.RotateSettings = Preset.RotateSettings;
        }

        public override void Execute(float deltaTime)
        {
            base.Execute(deltaTime);

            visual.RotateSettings = Preset.RotateSettings;
        }

        public void Look(Vector2 direction)
        {
            visual.Look(direction);
        }
    }
}