using System;
using StateMachines.Data;
using StateMachines.Presets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Conditions
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Values/Input Value", fileName = nameof(InputValue), order = 0)]
    public class InputValue : BoolValuePreset
    {
        public InputActionType InputActionType;
        public InputActionReference ActionReference;

        public override IValueData CreateProvider()
        {
            return new InputValueData();
        }
    }

    public sealed class InputValueData : ValueData<InputValue>
    {
        public override bool GetBool()
        {
            switch (Preset.InputActionType)
            {
                case InputActionType.Pressed: return Preset.ActionReference.action.WasPressedThisFrame();
                case InputActionType.Performed: return Preset.ActionReference.action.WasPerformedThisFrame();
                case InputActionType.Released: return Preset.ActionReference.action.WasReleasedThisFrame();
            }

            return false;
        }
    }

    public enum InputActionType
    {
        Pressed,
        Performed,
        Released,
    }
}