using System;
using StateMachines.Presets;
using StateMachines.Utils;

namespace StateMachines.Data
{
    public abstract class FeatureProcess<T> : IFeatureProcess where T : FeaturePreset
    {
        public StateMachineData StateMachine { get; private set; }
        public int StateId { get; private set; }
        public T Preset { get; private set; }
        
        public bool IsRunning { get; private set; }
        public float TimeInState { get; private set; }

        public void Bake(StateMachineData stateMachine, int stateId, FeaturePreset featurePreset)
        {
            StateMachine = stateMachine;
            StateId = stateId;
            Preset = featurePreset as T;
        }

        public virtual void Initialize()
        {
            Injector.InjectComponents(this, StateMachine.Context);
            Injector.InjectFeature(this, StateMachine, StateId);
        }
        
        public virtual void Enter()
        {
            IsRunning = true;
            TimeInState = 0f;
        }

        public virtual void Execute(float deltaTime)
        {
            TimeInState += deltaTime;
        }

        public virtual void Exit()
        {
            IsRunning = false;
        }
        
        public virtual void Dispose()
        {
            // TODO release managed resources here
        }
    }
    
    public interface IFeatureProcess : IDisposable
    {
        public bool IsRunning { get; }
        public float TimeInState { get; }
        
        public void Bake(StateMachineData stateMachine, int stateId, FeaturePreset featurePreset);
        public void Initialize();
        public void Enter();
        public void Execute(float deltaTime);
        public void Exit();
    }
}