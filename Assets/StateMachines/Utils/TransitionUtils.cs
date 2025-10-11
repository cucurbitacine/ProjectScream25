using System.Collections.Generic;
using System.Linq;
using StateMachines.Data;
using StateMachines.Presets;
using UnityEngine;

namespace StateMachines.Utils
{
    public static class TransitionUtils
    {
        public static TransitionData Bake(this TransitionPreset transitionPreset, int transitionId, ScriptableObject source, StateMachineData stateMachine, int originStateId, int targetStateId)
        {
            var transitionData = new TransitionData();
            
            transitionData.TransitionId = transitionId;
            transitionData.Source = source;
            
            transitionData.StateMachine = stateMachine;
            transitionData.OriginStateId = originStateId;
            transitionData.TargetStateId = targetStateId;
            
            transitionData.Solo = transitionPreset.Solo;
            transitionData.Mute = transitionPreset.Mute;

            transitionData.Conditions = new List<ConditionData>();
            transitionData.Commands = new List<ICommandExecutor>();

            foreach (var conditionPreset in transitionPreset.Conditions)
            {
                var conditionData = conditionPreset.Bake(stateMachine, originStateId);
                transitionData.Conditions.Add(conditionData);
            }

            foreach (var commandHolder in transitionPreset.Commands)
            {
                var commandData = commandHolder.Bake(stateMachine, originStateId);
                transitionData.Commands.Add(commandData);
            }

            return transitionData;
        }
        
        public static void Initialize(this TransitionData transitionData)
        {
            foreach (var conditionData in transitionData.Conditions)
            {
                conditionData.Initialize();
            }

            foreach (var command in transitionData.Commands)
            {
                command.Initialize();
            }
        }

        public static bool IsReady(this TransitionData transitionData)
        {
            if (transitionData.Solo) return true;
            if (transitionData.Mute) return false;

            return transitionData.Conditions.All(c => c.IsTrue());
        }
    }
}