using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Configs;
using Infrastructure.Services.Asset.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Popup.Core;
using Plugins.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.Popup
{
    public class PopupService : IPopupService, IInitializable, IDisposable
    {
        private readonly IAssetService _assetService;
        private readonly PopupServiceConfig _popupServiceConfig;
        private readonly IInputService _inputService;

        public PopupService(IAssetService assetService, PopupServiceConfig popupServiceConfig, IInputService inputService)
        {
            _assetService = assetService;
            _popupServiceConfig = popupServiceConfig;
            _inputService = inputService;
        }

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private Transform _container;
        private GameObject _inputBlocker;

        public void Initialize()
        {
            _container = CreateContainer();
            _inputBlocker = CreateInputBlocker();
        }

        public void Dispose()
        {
            DestroyAll();
            _cts.Cancel();
        }

        public async UniTask<IPopup> Show(PopupID id)
        {
            _inputBlocker.SetActive(true);

            GameObject previousSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(null);

            bool previousPlayerActionsState = _inputService.Actions.Player.enabled;

            _inputService.Actions.Player.Disable();

            GameObject popupInstance = await _assetService.InstantiateAsync(_popupServiceConfig.GetAssetReference(id));
            popupInstance.transform.SetParent(_container);

            RectTransform rectTransform = (RectTransform)popupInstance.transform;
            Maximize(rectTransform);

            IPopup popup = popupInstance.GetComponent<IPopup>();
            popup.Show().Forget();

            UniTask
                .WaitUntil(() => popupInstance == null, cancellationToken: _cts.Token)
                .ContinueWith(() =>
                {
                    if (previousPlayerActionsState)
                        _inputService.Actions.Player.Enable();

                    EventSystem.current?.SetSelectedGameObject(previousSelectedGameObject);

                    _inputBlocker.SetActive(false);
                })
                .Forget();

            return popup;
        }

        private void Maximize(RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        public void DestroyAll()
        {
            if (_container == null)
                return;

            GameObject[] children = _container.gameObject.GetChildren();

            foreach (GameObject child in children)
            {
                if (child != _inputBlocker)
                    Object.Destroy(child);
            }
        }

        private Transform CreateContainer()
        {
            GameObject instance = Object.Instantiate(_popupServiceConfig.ContainerPrefab);
            Object.DontDestroyOnLoad(instance);
            return instance.transform;
        }

        private GameObject CreateInputBlocker()
        {
            GameObject instance = Object.Instantiate(_popupServiceConfig.InputBlockerPrefab, _container, true);
            RectTransform rectTransform = (RectTransform)instance.transform;
            Maximize(rectTransform);
            instance.SetActive(false);
            return instance;
        }
    }
}