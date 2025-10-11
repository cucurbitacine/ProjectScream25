using System;
using System.Collections.Generic;

namespace StateMachines.Data
{
    public struct GroupData : IDisposable
    {
        public StateMachineData StateMachine;
        public int StateId;
        
        public List<TransitionData> Transitions;
        
        public void Dispose()
        {
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