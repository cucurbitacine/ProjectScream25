using StateMachines.Data;
using StateMachines.Presets;

namespace StateMachines.Utils
{
    public static class CommandUtils
    {
        public static ICommandExecutor Bake(this CommandPreset commandPreset, StateMachineData stateMachine, int stateId, Value value)
        {
            var commandExecutor = commandPreset.CreateExecutor();
            commandExecutor.Bake(stateMachine, stateId, commandPreset, value);
            return commandExecutor;
        }
        
        public static ICommandExecutor Bake(this CommandHolder commandHolder, StateMachineData stateMachine, int stateId)
        {
            return commandHolder.CommandPreset.Bake(stateMachine, stateId, commandHolder.value);
        }
    }
}