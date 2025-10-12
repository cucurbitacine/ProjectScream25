using System;
using StateMachines.Presets;
using StateMachines.Utils;

namespace StateMachines.Data
{
    public abstract class CommandExecutor<T> : ICommandExecutor where T : CommandPreset
    {
        public StateMachineData StateMachine { get; private set; }
        public int StateId { get; private set; }
        public T Preset { get; private set; }
        public Value Value { get; set; }
        
        public void Bake(StateMachineData stateMachine, int stateId, CommandPreset commandPreset, Value value)
        {
            StateMachine = stateMachine;
            StateId = stateId;
            Value = value;
            Preset = commandPreset as T;
        }

        public virtual void Initialize()
        {
            Injector.InjectComponents(this, StateMachine.Context);
            Injector.InjectProcesses(this, StateMachine, StateId);
        }

        public abstract void Execute();
        
        public virtual void Dispose()
        {
            // TODO release managed resources here
        }
    }
    
    public interface ICommandExecutor : IDisposable
    {
        public void Bake(StateMachineData stateMachine, int stateId, CommandPreset commandPreset, Value value);
        public void Initialize();
        public void Execute();
    }
}