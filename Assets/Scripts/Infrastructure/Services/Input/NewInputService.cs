using Infrastructure.Services.Input.Core;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using VContainer.Unity;

namespace Infrastructure.Services.Input
{
    public class NewInputService : IInputService, IInitializable
    {
        private readonly InputSystemUIInputModule _uiInputModule;

        public NewInputService(InputSystemUIInputModule uiInputModule)
        {
            _uiInputModule = uiInputModule;
        }

        private InputActions _inputActions;

        public InputActions Actions => _inputActions;

        public void Initialize()
        {
            _inputActions = new InputActions();
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            _uiInputModule.enabled = active;

            if (active)
                _inputActions.Enable();
            else
                _inputActions.Disable();
        }
    }
}