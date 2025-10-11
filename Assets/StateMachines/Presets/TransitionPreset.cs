using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Presets
{
    [Serializable]
    public class TransitionPreset
    {
        [HideInInspector] [SerializeField] private string displayName;

        public StatePreset Target;
        public List<ConditionPreset> Conditions;
        public List<CommandHolder> Commands;

        [Space]
        public bool Solo;
        public bool Mute;
        
        public void OnValidate(int index, string sourceStateName)
        {
            var targetStateName = Target ? Target.StateName : "No State";
            displayName = $"\"{sourceStateName}\" > \"{targetStateName}\"";

            if (Solo) displayName = $"[solo] {displayName}";
            else if (Mute) displayName = $"[mute] {displayName}";

            displayName = $"[{index}] {displayName}";
        }
    }
}