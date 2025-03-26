﻿using Cysharp.Threading.Tasks;
using Infrastructure.Configs;
using Infrastructure.Services.Asset.Core;
using Infrastructure.Services.Instantiate.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.Services.Window.Factories.Core;
using Plugins.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.Window.Factories
{
    public class WindowFactory : IWindowFactory
    {
        private readonly WindowFactoryConfig _config;
        private readonly IAssetService _assetService;
        private readonly IInstantiateService _instantiateService;

        public WindowFactory(WindowFactoryConfig config, IAssetService assetService, IInstantiateService instantiateService)
        {
            _config = config;
            _assetService = assetService;
            _instantiateService = instantiateService;
        }

        private RectTransform _uiRootRectTransform;

        public async UniTask<IWindow> CreateWindow(WindowID windowID)
        {
            _uiRootRectTransform = GetOrCreateUIRoot();

            InputBlocker inputBlocker = _instantiateService.Instantiate(_config.InputBlockerPrefab);
            inputBlocker.transform.SetParent(_uiRootRectTransform);
            inputBlocker.RectTransform.Maximize();

            AssetReferenceGameObject windowReference = _config.WindowsMap[windowID];

            GameObject windowInstance = await _assetService.InstantiateAsync(windowReference);
            IWindow window = windowInstance.GetComponent<IWindow>();
            window.RectTransform.SetParent(_uiRootRectTransform);
            window.RectTransform.Maximize();

            inputBlocker.LinkTo(window.RectTransform.gameObject);
            inputBlocker.RectTransform.SetBefore(window.RectTransform);

            return window;
        }

        private RectTransform GetOrCreateUIRoot()
        {
            if (_uiRootRectTransform == null)
            {
                GameObject instance = _instantiateService.Instantiate(_config.ContainerPrefab);
                return instance.transform as RectTransform;
            }

            return _uiRootRectTransform;
        }
    }
}