using Infrastructure.Services.Input.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Services.Input.NewInputSystem
{
    public class PlayerInputActions : IPlayerInputActions
    {
        public PlayerInputActions(InputActions.PlayerActions playerActions)
        {
            Move = new FuncInputAction<Vector2>(() => playerActions.Move.ReadValue<Vector2>());
            Look = new FuncInputAction<Vector2>(() => playerActions.Look.ReadValue<Vector2>());
            Fire = new FuncInputAction<bool>(() => playerActions.Fire.ReadValue<bool>());
        }

        public IInputAction<Vector2> Move { get; }
        public IInputAction<Vector2> Look { get; }
        public IInputAction<bool> Fire { get; }

        public void SetActive(bool active)
        {
            Move.Enabled = active;
            Look.Enabled = active;
            Fire.Enabled = active;
        }
    }
}