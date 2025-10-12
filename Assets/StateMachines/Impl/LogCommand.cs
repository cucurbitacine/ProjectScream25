using StateMachines.Data;
using StateMachines.Presets;
using UnityEngine;

namespace StateMachines.Impl
{
    [CreateAssetMenu(menuName = StateMachinePreset.CreateCommandMenu + "/Log", fileName = "Command Log", order = 0)]
    public class LogCommand : CommandPreset
    {
        public bool Mute;
        public LogType LogType = LogType.Log;
        public string Prefix;
        
        public override ICommandExecutor CreateExecutor()
        {
            return new LogExecutor();
        }
    }

    public sealed class LogExecutor : CommandExecutor<LogCommand>
    {
        public override void Execute()
        {
            if (Preset.Mute) return;
            
            var log = Value.StringValue;

            log = $"[{StateMachine.Context.name}] {log}";
            
            if (!string.IsNullOrWhiteSpace(Preset.Prefix))
            {
                log = $"{Preset.Prefix} {log}";
            }
            
            Debug.LogFormat(Preset.LogType, LogOption.None, StateMachine.Context, log);
        }
    }
}