using Game.Scripts.Control;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Features
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Features/Wander Feature", fileName = nameof(WanderFeature), order = 0)]
    public class WanderFeature : FeaturePreset
    {
        [Min(0f)] public float WanderRadius = 5f;
        [Min(0f)] public float WanderDelay = 2f;
        [Min(0f)] public float StopDuration = 2f;
        
        public override IFeatureProcess CreateProcess()
        {
            return new WanderProcess();
        }
    }

    public sealed class WanderProcess : FeatureProcess<WanderFeature>
    {
        [InjectProcess] private MovementProcess movement;
        [InjectProcess] private RotationProcess rotation;

        private Vector2 randomPoint;
        private float timer;

        private Navigator navigator;

        public override void Enter()
        {
            base.Enter();

            if (navigator == null) navigator = new Navigator();

            navigator.CalculatePath(movement.Position, GetRandomPoint());
            
            //randomPoint = GetRandomPoint();
            timer = Preset.WanderDelay;
        }

        public override void Execute(float deltaTime)
        {
            base.Execute(deltaTime);
            
            if (timer > 0f)
            {
                movement.Move(Vector2.zero);
                timer -= deltaTime;
                return;
            }

            if (navigator.Path.corners.Length > 0)
            {
                for (var i = 0; i < navigator.Path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(navigator.Path.corners[i], navigator.Path.corners[i + 1], Color.softRed, 2f);
                }
            }
            
            if (navigator.GetNextPoint(movement.Position, out var nextPoint))
            {
                movement.Move(nextPoint - movement.Position);
                rotation.Look(movement.VelocityActual);
            }
            else
            {
                movement.Move(Vector2.zero);
                
                navigator.CalculatePath(movement.Position, GetRandomPoint());
                timer = Preset.StopDuration;
            }
        }

        private Vector2 GetRandomPoint()
        {
            return movement.Position + Random.insideUnitCircle * Preset.WanderRadius;
        }
    }
}
