using System;
using StateMachines.Presets;
using StateMachines.Utils;

namespace StateMachines.Data
{
    public class ValueData<T> : IValueData where T : ValuePreset
    {
        public StateMachineData StateMachine { get; private set; }
        public int StateId { get; private set; }
        public T Preset { get; private set; }

        public virtual bool GetBool()
        {
            return false;
        }

        public virtual int GetInt()
        {
            return 0;
        }

        public virtual float GetFloat()
        {
            return 0f;
        }

        public string GetString()
        {
            return string.Empty;
        }

        public void Bake(StateMachineData stateMachine, int stateId, ValuePreset valuePreset)
        {
            StateMachine = stateMachine;
            StateId = stateId;
            Preset = valuePreset as T;
        }

        public virtual void Initialize()
        {
            Injector.InjectComponents(this, StateMachine.Context);
            Injector.InjectFeature(this, StateMachine, StateId);
        }

        public virtual void Dispose()
        {
            // TODO release managed resources here
        }
    }
    
    public interface IValueData : IDisposable
    {
        public bool GetBool();
        public int GetInt();
        public float GetFloat();
        public string GetString();

        public void Bake(StateMachineData stateMachine, int stateId, ValuePreset valuePreset);
        public void Initialize();
    }
}