using System;
using System.Collections.Generic;

namespace StateMachines.Data
{
    public struct StateData : IDisposable
    {
        public StateMachineData StateMachine;
        public int StateId;
        
        public List<ICommandExecutor> CommandsOnEnter;
        public List<ICommandExecutor> CommandsOnExit;
        public List<IFeatureProcess> Features;

        public List<GroupData> Groups;
        public List<TransitionData> Transitions;

        public void Dispose()
        {
            if (CommandsOnEnter != null)
            {
                foreach (var invoker in CommandsOnEnter)
                {
                    invoker.Dispose();
                }

                CommandsOnEnter.Clear();
                CommandsOnEnter = null;
            }

            if (CommandsOnExit != null)
            {
                foreach (var invoker in CommandsOnExit)
                {
                    invoker.Dispose();
                }

                CommandsOnExit.Clear();
                CommandsOnExit = null;
            }

            if (Features != null)
            {
                foreach (var process in Features)
                {
                    process.Dispose();
                }

                Features.Clear();
                Features = null;
            }

            if (Groups != null)
            {
                foreach (var group in Groups)
                {
                    group.Dispose();
                }

                Groups.Clear();
                Groups = null;
            }

            if (Transitions != null)
            {
                foreach (var transition in Transitions)
                {
                    transition.Dispose();
                }

                Transitions.Clear();
                Transitions = null;
            }
        }
    }
}