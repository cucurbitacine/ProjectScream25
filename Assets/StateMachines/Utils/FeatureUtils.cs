using StateMachines.Data;
using StateMachines.Presets;

namespace StateMachines.Utils
{
    public static class FeatureUtils
    {
        public static IFeatureProcess Bake(this FeaturePreset featurePreset, StateMachineData stateMachine, int stateId)
        {
            var featureProcess = featurePreset.CreateProcess();
            featureProcess.Bake(stateMachine, stateId, featurePreset);
            return featureProcess;
        }
    }
}