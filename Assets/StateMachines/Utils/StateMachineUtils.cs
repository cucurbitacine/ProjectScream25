using System;
using System.Collections.Generic;
using System.Linq;
using StateMachines.Data;
using StateMachines.Presets;
using UnityEngine;

namespace StateMachines.Utils
{
    public static class StateMachineUtils
    {
        public static StateMachineData Bake(this StateMachinePreset stateMachinePreset, GameObject context)
        {
            stateMachinePreset.Validate();
            
            var stateMachineData = new StateMachineData();
            
            stateMachineData.Context = context;

            stateMachineData.States = new List<StateData>();
            foreach (var statePreset in stateMachinePreset.States)
            {
                var stateData = statePreset.Bake(stateMachineData, statePreset.StateId);
                stateMachineData.States.Add(stateData);
            }

            return stateMachineData;
        }
        
        public static bool TryGetFeature(this StateMachineData stateMachine, int stateId, Type featureType, out IFeatureProcess feature)
        {
            if (0 <= stateId && stateId < stateMachine.States.Count)
            {
                feature = stateMachine.States[stateId].Features.FirstOrDefault(f => f.GetType() == featureType);
                return feature != null;
            }

            feature = null;
            return false;
        }
        
        public static bool TryGetFeature<T>(this StateMachineData stateMachine, int stateId, out T feature) where T : IFeatureProcess
        {
            if (0 <= stateId && stateId < stateMachine.States.Count)
            {
                feature = stateMachine.States[stateId].Features.OfType<T>().FirstOrDefault();
                return feature != null;
            }
            
            feature = default;
            return false;
        }
    }
}