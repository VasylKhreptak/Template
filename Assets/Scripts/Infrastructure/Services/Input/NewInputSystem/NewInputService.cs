using Infrastructure.Services.Input.Core;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using VContainer.Unity;

namespace Infrastructure.Services.Input.NewInputSystem
{
    public class NewInputService : IInputService, IInitializable
    {
        private readonly InputSystemUIInputModule _uiInputModule;

        public NewInputService(InputSystemUIInputModule uiInputModule)
        {
            _uiInputModule = uiInputModule;
        }

        private InputActions _inputActions;

        public IPlayerInputActions Player { get; private set; }
        public IUIInputActions UI { get; private set; }

        public void Initialize()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();
            Player = new PlayerInputActions(_inputActions.Player);
            UI = new UIInputActions(_inputActions.UI);

            SetActive(false);
        }

        public void SetActive(bool active)
        {
            _uiInputModule.enabled = active;
            Player.SetActive(active);
            UI.SetActive(active);
        }
    }
}