using Infrastructure.Services.Input.Core;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using VContainer.Unity;

namespace Infrastructure.Services.Input
{
    public class InputService : IInputService, IInitializable
    {
        private readonly EventSystem _eventSystem;
        private readonly InputSystemUIInputModule _uiInputModule;

        public InputService(EventSystem eventSystem, InputSystemUIInputModule uiInputModule)
        {
            _eventSystem = eventSystem;
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
            _eventSystem.enabled = active;
            _uiInputModule.enabled = active;

            if (active)
                _inputActions.Enable();
            else
                _inputActions.Disable();
        }
    }
}