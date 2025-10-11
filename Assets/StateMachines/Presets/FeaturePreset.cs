using StateMachines.Data;
using UnityEngine;

namespace StateMachines.Presets
{
    public abstract class FeaturePreset : ScriptableObject 
    {
        public abstract IFeatureProcess CreateProcess();
    }
}