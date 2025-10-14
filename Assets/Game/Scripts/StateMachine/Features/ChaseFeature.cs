using Game.Scripts.Combat;
using Game.Scripts.Control;
using Game.Scripts.Utils;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Features
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Features/Chase Feature", fileName = "ChaseFeature", order = 0)]
    public class ChaseFeature : FeaturePreset
    {
        [Min(0f)]
        public float FrequencyTargetPositionUpdate = 2;

        public float PeriodTargetPositionUpdate =>
            FrequencyTargetPositionUpdate > 0f ? 1f / FrequencyTargetPositionUpdate : float.MaxValue;
        
        public override IFeatureProcess CreateProcess()
        {
            return new ChaseProcess();
        }
    }

    public sealed class ChaseProcess : FeatureProcess<ChaseFeature>
    {
        [InjectProcess] private MovementProcess movement;
        [InjectProcess] private RotationProcess rotation;
        [InjectComponent] private TargetSelectController targetSelect;

        private Navigator navigator;
        private Vector2 actualTargetPoint;
        private float lastTargetPositionUpdateTime;
        
        private float timeSinceLastTargetPositionUpdate => TimeInState - lastTargetPositionUpdateTime;
        
        public override void Enter()
        {
            base.Enter();

            navigator = new Navigator();
            lastTargetPositionUpdateTime = float.MaxValue;
        }

        public override void Execute(float deltaTime)
        {
            base.Execute(deltaTime);

            if (!targetSelect.HasTarget) return;

            if (timeSinceLastTargetPositionUpdate > Preset.PeriodTargetPositionUpdate)
            {
                actualTargetPoint = GetActualTargetPoint();
                navigator.CalculatePath(movement.Position, actualTargetPoint);
            }
            
            if (navigator.GetNextPoint(movement.Position, out var nextPoint))
            {
                var direction = nextPoint - movement.Position;
                if (direction.sqrMagnitude > 0)
                    movement.Move(direction.normalized);
            }
            else
            {
                //movement.Move(Vector2.zero);
                
                actualTargetPoint = GetActualTargetPoint();
                navigator.CalculatePath(movement.Position, actualTargetPoint);
            }
            
            rotation.LookAt(actualTargetPoint);
        }

        private Vector2 GetActualTargetPoint()
        {
            return targetSelect.HasTarget ? targetSelect.Target.transform.position : movement.Position;
        }
    }
}