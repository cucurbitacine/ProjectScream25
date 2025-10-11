using System;
using System.Collections.Generic;
using StateMachines.Utils;
using UnityEngine;

namespace StateMachines.Data
{
    [Serializable]
    public class StateMachineData : IDisposable
    {
        [HideInInspector] public GameObject Context;
        [HideInInspector] public List<StateData> States;

        [Space]
        public float TimeInState;
        public int FrameInState;

        [HideInInspector] public int CurrentStateId;
        [HideInInspector] public int PreviousStateId;
        [HideInInspector] public int LastTransitionId;
        [HideInInspector] public ScriptableObject LastTransitionSource;
        
        private Dictionary<Type, Component> _componentCache;
        
        public void Initialize()
        {
            if (_componentCache == null)
            {
                _componentCache = new Dictionary<Type, Component>();
            }
            else
            {
                _componentCache.Clear();
            }
            
            PreviousStateId = -1;
            CurrentStateId = 0;
            
            for (var i = 0; i < States.Count; i++)
            {
                States[i].Initialize();
            }
        }
        
        public void Enter()
        {
            EnterState(CurrentStateId);
        }
        
        public void Execute(float deltaTime)
        {
            HandleTransition();

            ExecuteState(CurrentStateId, deltaTime);
        }

        public void Exit()
        {
            ExitState(CurrentStateId);
            
            TimeInState = 0f;
            FrameInState = 0;
            
            CurrentStateId = 0;
            PreviousStateId = -1;
            
            LastTransitionId = -1;
            LastTransitionSource = null;
        }
        
        public bool TryGetComponent<T>(out T result) where T : Component
        {
            var type = typeof(T);
            
            if (_componentCache.TryGetValue(type, out var component))
            {
                return (result = component as T) != null;
            }

            if (Context.TryGetComponent(out result))
            {
                _componentCache.Add(type, result);
            }

            return result != null;
        }
        
        public void Dispose()
        {
            _componentCache?.Clear();
            _componentCache = null;

            if (States != null)
            {
                foreach (var state in States)
                {
                    state.Dispose();
                }
                
                States.Clear();
                States = null;
            }
        }
        
        private void EnterState(int stateId)
        {
            var stateData = States[stateId];
            
            foreach (var command in stateData.CommandsOnEnter)
            {
                command.Execute();
            }

            foreach (var feature in stateData.Features)
            {
                feature.Enter();
            }
            
            TimeInState = 0f;
            FrameInState = 0;
        }

        private void ExecuteState(int stateId, float deltaTime)
        {
            var stateData = States[stateId];
            
            foreach (var feature in stateData.Features)
            {
                feature.Execute(deltaTime);
            }

            TimeInState += deltaTime;
            FrameInState += 1;
        }
        
        private void ExitState(int stateId)
        {
            var stateData = States[stateId];
            
            foreach (var feature in stateData.Features)
            {
                feature.Exit();
            }
            
            foreach (var command in stateData.CommandsOnExit)
            {
                command.Execute();
            }
        }
        
        private void HandleTransition()
        {
            var currentStateData = States[CurrentStateId];
            
            if (currentStateData.GetReadyTransition(out var transition))
            {
                ExitState(CurrentStateId);

                PreviousStateId = CurrentStateId;
                CurrentStateId = transition.TargetStateId;
                
                LastTransitionId = transition.TransitionId;
                LastTransitionSource = transition.Source;
                    
                foreach (var command in transition.Commands)
                {
                    command.Execute();
                }
                
                EnterState(CurrentStateId);
            }
        }
    }
}