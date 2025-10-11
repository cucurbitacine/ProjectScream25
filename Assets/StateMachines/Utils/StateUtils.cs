using System.Collections.Generic;
using StateMachines.Data;
using StateMachines.Presets;

namespace StateMachines.Utils
{
    public static class StateUtils
    {
        public static StateData Bake(this StatePreset statePreset, StateMachineData stateMachine, int stateId)
        {
            var stateData = new StateData();
            
            stateData.StateMachine = stateMachine;
            stateData.StateId = stateId;

            stateData.CommandsOnEnter = new List<ICommandExecutor>();
            stateData.CommandsOnExit = new List<ICommandExecutor>();
            stateData.Features = new List<IFeatureProcess>();
            stateData.Groups = new List<GroupData>();
            stateData.Transitions = new List<TransitionData>();
            
            foreach (var commandHolder in statePreset.CommandsOnEnter)
            {
                var commandData = commandHolder.Bake(stateMachine, stateId);
                stateData.CommandsOnEnter.Add(commandData);
            }
            
            foreach (var commandHolder in statePreset.CommandsOnExit)
            {
                var commandData = commandHolder.Bake(stateMachine, stateId);
                stateData.CommandsOnExit.Add(commandData);
            }
            
            foreach (var featurePreset in statePreset.Features)
            {
                var featureData = featurePreset.Bake(stateMachine, stateId);
                stateData.Features.Add(featureData);
            }

            foreach (var groupPreset in statePreset.Groups)
            {
                var groupData = groupPreset.Bake(stateMachine, stateId);
                stateData.Groups.Add(groupData);
            }

            for (var transitionId = 0; transitionId < statePreset.Transitions.Count; transitionId++)
            {
                var transitionPreset = statePreset.Transitions[transitionId];
                var targetStateId = transitionPreset.Target.StateId;
                var transitionData = transitionPreset.Bake(transitionId, statePreset, stateMachine, stateId, targetStateId);
                stateData.Transitions.Add(transitionData);
            }

            return stateData;
        }

        public static void Initialize(this StateData stateData)
        {
            foreach (var command in stateData.CommandsOnEnter)
            {
                command.Initialize();
            }

            foreach (var command in stateData.CommandsOnExit)
            {
                command.Initialize();
            }

            foreach (var feature in stateData.Features)
            {
                feature.Initialize();
            }

            foreach (var group in stateData.Groups)
            {
                group.Initialize();
            }
            
            foreach (var transition in stateData.Transitions)
            {
                transition.Initialize();
            }
        }
        
        public static bool GetReadyTransition(this StateData stateData, out TransitionData transition)
        {
            foreach (var group in stateData.Groups)
            {
                if (group.GetReadyTransition(out transition))
                {
                    return true;
                }
            }
            
            foreach (var handler in stateData.Transitions)
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