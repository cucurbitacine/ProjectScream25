using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Presets
{
    [CreateAssetMenu(menuName = StateMachinePreset.StateMachineMenu + "/Group", fileName = "Group", order = 0)]
    public sealed class GroupPreset : ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; private set; }
        [field: SerializeField] public List<TransitionPreset> Transitions { get; private set; } = new List<TransitionPreset>();
        
        private void Reset()
        {
            GroupName = name;
        }
        
        private void OnValidate()
        {
            for (var i = 0; i < Transitions.Count; i++)
            {
                Transitions[i]?.OnValidate(i, GroupName);
            }

            if (string.IsNullOrWhiteSpace(GroupName))
            {
                GroupName = name;
            }
        }
    }
}