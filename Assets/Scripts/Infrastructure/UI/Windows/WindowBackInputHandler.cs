using Infrastructure.Optimization;
using Infrastructure.Services.FixedTickable.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.LateTickable.Core;
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
        public void Construct(ITickableService tickableService, IFixedTickableService fixedTickableService, ILateTickableService lateTickableService,
            IInputService inputService, IWindowService windowService)
        {
            base.Construct(tickableService, fixedTickableService, lateTickableService);

            _inputService = inputService;
            _windowService = windowService;
        }

        #region MonoBehaviour

        private void OnValidate() => _window ??= GetComponent<IWindow>();

        #endregion

        public void Tick()
        {
            if (_inputService.UI.Cancel.Value &&
                _window.RootCanvasGroup.interactable &&
                _windowService.TopWindow.Value == _window &&
                _windowService.IsLoadingAnyWindow == false)
            {
                _window.Hide();
            }
        }
    }
}