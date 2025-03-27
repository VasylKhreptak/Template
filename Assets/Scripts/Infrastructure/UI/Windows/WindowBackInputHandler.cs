using Infrastructure.Optimization;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Tickable.Core;
using Infrastructure.Services.Window.Core;
using UnityEngine;
using VContainer;

namespace Infrastructure.UI.Windows
{
    public class WindowBackInputHandler : CachedSerializedMonoBehaviour, ITickable
    {
        [Header("References")]
        [SerializeField] private IWindow _window;

        private IInputService _inputService;
        private IWindowService _windowService;

        [Inject]
        public void Construct(IInputService inputService, IWindowService windowService)
        {
            _inputService = inputService;
            _windowService = windowService;
        }

        private void OnValidate() => _window ??= GetComponent<IWindow>();

        public void Tick()
        {
            if (_inputService.Actions.UI.Cancel.WasPerformedThisFrame() && _window.RootCanvasGroup.interactable && _windowService.TopWindow.Value == _window)
                _window.Hide();
        }
    }
}