using System;
using Infrastructure.Services.Window.Core;
using UniRx;
using UnityEngine;
using VContainer;

namespace Infrastructure.UI.Tools
{
    public class ZeroLayerCanvasInteractionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        private IDisposable _subscription;

        private IWindowService _windowService;

        [Inject]
        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }

        #region MonoBehaviour

        private void OnValidate() => _canvasGroup ??= GetComponent<CanvasGroup>();

        private void OnEnable() => _subscription = _windowService.TopWindow.Subscribe(OnTopWindowChanged);

        private void OnDisable() => _subscription?.Dispose();

        #endregion

        private void OnTopWindowChanged(IWindow window) => _canvasGroup.interactable = window == null;
    }
}