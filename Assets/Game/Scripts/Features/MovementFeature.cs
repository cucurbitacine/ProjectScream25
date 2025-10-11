using System.Linq;
using Game.Scripts.Control;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.Features
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Features/Movement Feature", fileName = nameof(MovementFeature), order = 0)]
    public class MovementFeature : FeaturePreset
    {
        public AnimationCurve SpeedMaxPerAngle = AnimationCurve.Constant(0f, 1f, 1f);
        public MoveSettings MoveSettings = MoveSettings.Default;
        
        public override IFeatureProcess CreateProcess()
        {
            return new MovementProcess();
        }
    }

    public sealed class MovementProcess : FeatureProcess<MovementFeature>
    {
        [InjectComponent] private KinematicBody kinematic;
        [InjectComponent] private VisualBody visual;

        public override void Enter()
        {
            base.Enter();
            
            kinematic.MoveSettings = Preset.MoveSettings;
        }

        public override void Execute(float deltaTime)
        {
            base.Execute(deltaTime);

            var angle = Vector2.Angle(visual.DirectionActual, kinematic.VelocityActual);
            var speedFactor = Preset.SpeedMaxPerAngle.Evaluate(angle / 180f);

            var moveSettings = Preset.MoveSettings;
            moveSettings.speedMax *= speedFactor;
            
            kinematic.MoveSettings = moveSettings;
        }
        
        public void Move(Vector2 move)
        {
            kinematic.Move(move);
        }
    }
}