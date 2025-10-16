using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Input
{
    [CreateAssetMenu(fileName = "UIInput", menuName = "Scriptable Objects/Input/UIInput")]
    public class UIInput : ScriptableObject, GameInput.IUIActions
    {
        [field: SerializeField] public bool Submit { get; private set; }
        [field: SerializeField] public bool Cancel { get; private set; }
        [field: SerializeField] public Vector2 Point { get; private set; }

        public Action<bool> Submitted;
        public Action<bool> Canceled;
        
        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            Submit = context.performed;

            if (context.performed)
            {
                Submitted?.Invoke(true);
            }
            else if (context.canceled)
            {
                Submitted?.Invoke(false);
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            Cancel = context.performed;

            if (context.performed)
            {
                Canceled?.Invoke(true);
            }
            else if (context.canceled)
            {
                Canceled?.Invoke(false);
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            Point = context.ReadValue<Vector2>();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
        }
        
        private GameInput gameInput;
        
        private void OnEnable()
        {
            if (gameInput == null) gameInput = new GameInput();
            
            gameInput.UI.SetCallbacks(this);
            gameInput.UI.Enable();
        }

        private void OnDisable()
        {
            gameInput.UI.RemoveCallbacks(this);
            gameInput.UI.Disable();
        }
    }
}
