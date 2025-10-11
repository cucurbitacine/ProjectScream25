using System;
using StateMachines.Data;
using UnityEngine;

namespace StateMachines.Presets
{
    public abstract class CommandPreset : ScriptableObject
    {
        public abstract ICommandExecutor CreateExecutor();
    }

    [Serializable]
    public struct CommandHolder
    {
        public CommandPreset CommandPreset;
        public Value value;
    }
}