using System;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace StateMachines
{
    public class StateMachineEngine : MonoBehaviour, IDisposable
    {
        [field: SerializeField] public bool IsRunning { get; private set; }
        [field: SerializeField] public bool AutoRun { get; private set; } = true;
        
        [field: Space]
        [field: SerializeField] public StateMachinePreset StateMachinePreset { get; private set; }

        [Space]
        [SerializeField] private StateMachineData stateMachine;

        [Header("Runtime")]
        [SerializeField] private StatePreset currentState;
        [SerializeField] private StatePreset previousState;
        [SerializeField] private int lastTransitionId;
        [SerializeField] private ScriptableObject lastTransitionSource;
        
        public void Run()
        {
            if (IsRunning) return;
            
            IsRunning = true;
            
            stateMachine.Enter();
        }

        public void Stop()
        {
            if (!IsRunning) return;
            
            stateMachine.Exit();
            
            IsRunning = false;
        }

        public void Dispose()
        {
            stateMachine?.Dispose();
        }
        
        private void Prepare()
        {
            stateMachine = StateMachinePreset.Bake(gameObject);
            
            stateMachine.Initialize();
        }
        
        private void Running(float deltaTime)
        {
            if (IsRunning)
            {
                stateMachine.Execute(deltaTime);

                if (stateMachine.CurrentStateId >= 0)
                {
                    currentState = StateMachinePreset.States[stateMachine.CurrentStateId];
                }

                if (stateMachine.PreviousStateId >= 0)
                {
                    previousState = StateMachinePreset.States[stateMachine.PreviousStateId];
                }
                lastTransitionId = stateMachine.LastTransitionId;
                lastTransitionSource = stateMachine.LastTransitionSource;
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            Prepare();
        }

        private void Start()
        {
            if (AutoRun) Run();
        }

        private void Update()
        {
            Running(Time.deltaTime);
        }
        
        private void OnDestroy()
        {
            Stop();
            
            Dispose();
        }

        #endregion
    }
}