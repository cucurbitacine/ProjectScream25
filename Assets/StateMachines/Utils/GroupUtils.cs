using System.Collections.Generic;
using StateMachines.Data;
using StateMachines.Presets;

namespace StateMachines.Utils
{
    public static class GroupUtils
    {
        public static GroupData Bake(this GroupPreset groupPreset, StateMachineData stateMachine, int stateId)
        {
            var groupData = new GroupData();
            
            groupData.StateMachine = stateMachine;
            groupData.StateId = stateId;

            groupData.Transitions = new List<TransitionData>();

            for (var transitionId = 0; transitionId < groupPreset.Transitions.Count; transitionId++)
            {
                var transitionPreset = groupPreset.Transitions[transitionId];
                var targetStateId = transitionPreset.Target.StateId;
                var transitionData = transitionPreset.Bake(transitionId, groupPreset, stateMachine, stateId, targetStateId);
                
                groupData.Transitions.Add(transitionData);
            }

            return groupData;
        }
        
        public static void Initialize(this GroupData groupData)
        {
            foreach (var transition in groupData.Transitions)
            {
                transition.Initialize();
            }
        }
        
        public static bool GetReadyTransition(this GroupData groupData, out TransitionData transition)
        {
            foreach (var handler in groupData.Transitions)
            {
                if (handler.IsReady())
                {
                    transition = handler;
                    return true;
                }
            }
            
            transition = default;
            return false;
        }
    }
}