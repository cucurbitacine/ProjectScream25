using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Data
{
    public struct TransitionData : IDisposable
    {
        public int TransitionId;
        public ScriptableObject Source;

        public StateMachineData StateMachine;
        public int OriginStateId;
        public int TargetStateId;
        public bool Solo;
        public bool Mute;

        public List<ConditionData> Conditions;
        public List<ICommandExecutor> Commands;
        
        public void Dispose()
        {
            if (Conditions != null)
            {
                foreach (var condition in Conditions)
                {
                    condition.Dispose();
                }

                Conditions.Clear();
                Conditions = null;
            }

            if (Commands != null)
            {
                foreach (var command in Commands)
                {
                    command.Dispose();
                }

                Commands.Clear();
                Commands = null;
            }
        }
    }
}