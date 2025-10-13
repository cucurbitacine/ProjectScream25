using Game.Scripts.Combat;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Features
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Features/Target Select Feature", fileName = "TargetSelectFeature", order = 0)]
    public class TargetSelectFeature : FeaturePreset
    {
        public override IFeatureProcess CreateProcess()
        {
            return new TargetSelectProcess();
        }
    }

    public sealed class TargetSelectProcess : FeatureProcess<TargetSelectFeature>
    {
        [InjectComponent] private TargetSelectController targetSelect;
        
        public override void Enter()
        {
            base.Enter();

            targetSelect.Begin();
        }

        public override void Exit()
        {
            base.Exit();
            
            targetSelect.End();
        }
    }
}