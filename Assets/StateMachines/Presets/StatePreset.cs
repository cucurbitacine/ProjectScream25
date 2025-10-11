using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Presets
{
    [CreateAssetMenu(menuName = StateMachinePreset.StateMachineMenu + "/State", fileName = "State", order = 0)]
    public sealed class StatePreset : ScriptableObject
    {
        public int StateId;
        public string StateName;
        public List<GroupPreset> Groups;
        
        [Space]
        public List<CommandHolder> CommandsOnEnter;
        public List<FeaturePreset> Features;
        public List<CommandHolder> CommandsOnExit;
        
        [Space] 
        public List<TransitionPreset> Transitions;

        private void Reset()
        {
            StateName = name;
        }

        private void OnValidate()
        {
            for (var i = 0; i < Transitions.Count; i++)
            {
                Transitions[i]?.OnValidate(i, StateName);
            }

            if (string.IsNullOrWhiteSpace(StateName))
            {
                StateName = name;
            }
        }
    }
}