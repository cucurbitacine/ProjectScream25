using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Presets
{
    [CreateAssetMenu(menuName = StateMachineMenu + "/State Machine", fileName = "StateMachine", order = 0)]
    public class StateMachinePreset : ScriptableObject
    {
        public const string StateMachineMenu = "State Machine";
        public const string CreateValueMenu = StateMachineMenu + "/Values";
        public const string CreateCommandMenu = StateMachineMenu + "/Commands";

        public List<StatePreset> States = new List<StatePreset>();

        public void Validate()
        {
            for (var stateId = 0; stateId < States.Count; stateId++)
            {
                States[stateId].StateId = stateId;
            }
        }
    }
}