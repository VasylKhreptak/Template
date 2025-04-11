using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Infrastructure.Services.Window
{
    public class WindowDebugger : MonoBehaviour
    {
        private IWindowService _windowService;

        [Inject]
        public void Construct(IWindowService windowService) => _windowService = windowService;

        [Button]
        private void CreateWindow(WindowID id) => _windowService.CreateWindow(id).ContinueWith(window => window.Show()).Forget();

        [Button]
        private void DestroyTopWindow()
        {
            IWindow window = _windowService.GetTopWindow();

            if (window != null)
                Destroy(window.RootRectTransform.gameObject);
        }
    }
}