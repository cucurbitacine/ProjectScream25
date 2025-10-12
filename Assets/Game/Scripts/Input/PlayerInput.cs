using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Input
{
    [CreateAssetMenu(fileName = nameof(PlayerInput), menuName = "Scriptable Objects/Inputs/Player Input")]
    public class PlayerInput : ScriptableObject, GameInput.IPlayerActions
    {
        [field: SerializeField] public Vector2 Move { get; private set; } 
        [field: SerializeField] public Vector2 Look { get; private set; }

        [field: Space]
        [field: SerializeField] public bool Attack { get; private set; } 
        [field: SerializeField] public bool Aim { get; private set; } 
        [field: SerializeField] public bool Interact { get; private set; } 
        [field: SerializeField] public bool Sprint { get; private set; } 

        public event Action Attacked;
        public event Action Interacted;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            Attack = context.performed;

            if (Attack) Attacked?.Invoke();
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            Aim = context.performed;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Interact = context.performed;

            if (Interact) Interacted?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
        }

        public void OnNext(InputAction.CallbackContext context)
        {
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            Sprint = context.performed;
        }

        private GameInput gameInput;
        
        private void OnEnable()
        {
            if (gameInput == null) gameInput = new GameInput();
            
            gameInput.Player.SetCallbacks(this);
            gameInput.Player.Enable();
        }

        private void OnDisable()
        {
            gameInput.Player.RemoveCallbacks(this);
            gameInput.Player.Disable();
        }
    }

    public enum ButtonState
    {
        Idle,
        
    }
}
