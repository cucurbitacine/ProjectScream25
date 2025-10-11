using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.Features
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Wander Feature", fileName = nameof(WanderFeature), order = 0)]
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
        
        public override void Enter()
        {
            base.Enter();

            randomPoint = movement.Position + Random.insideUnitCircle * Preset.WanderRadius;
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
            
            var direction = randomPoint - movement.Position;

            if (direction.sqrMagnitude < 0.1f)
            {
                randomPoint = movement.Position + Random.insideUnitCircle * Preset.WanderRadius;
                timer = Preset.StopDuration;
            }
            else
            {
                movement.Move(direction);
                rotation.Look(movement.VelocityActual);
            }
        }
    }
}
