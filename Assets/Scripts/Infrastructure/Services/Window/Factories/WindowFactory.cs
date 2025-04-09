using Cysharp.Threading.Tasks;
using Infrastructure.Configs;
using Infrastructure.Services.Asset.Core;
using Infrastructure.Services.Instantiate.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.Services.Window.Factories.Core;
using Plugins.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace Infrastructure.Services.Window.Factories
{
    public class WindowFactory : IWindowFactory
    {
        private readonly WindowFactoryConfig _config;
        private readonly IAssetService _assetService;
        private readonly IInstantiateService _instantiateService;
        private readonly LifetimeScope _lifetimeScope;

        public WindowFactory(WindowFactoryConfig config, IAssetService assetService, IInstantiateService instantiateService, LifetimeScope lifetimeScope)
        {
            _config = config;
            _assetService = assetService;
            _instantiateService = instantiateService;
            _lifetimeScope = lifetimeScope;
        }

        private RectTransform _uiRootRectTransform;

        public async UniTask<IWindow> CreateWindow(WindowID windowID)
        {
            RectTransform uiRoot = GetOrCreateUIRoot();

            GameObject inputBlocker = _instantiateService.Instantiate(_config.InputBlockerPrefab);
            inputBlocker.transform.SetParent(uiRoot);
            RectTransform inputBlockerRectTransform = (RectTransform)inputBlocker.transform;
            inputBlockerRectTransform.Maximize();

            AssetReferenceGameObject windowReference = _config.WindowsMap[windowID];

            GameObject windowInstance = await _assetService.InstantiateAsync(windowReference);
            IWindow window = windowInstance.GetComponent<IWindow>();
            window.RootRectTransform.SetParent(uiRoot);
            window.RootRectTransform.Maximize();

            inputBlocker.transform.SetParent(window.RootRectTransform);
            inputBlocker.transform.SetAsFirstSibling();
            inputBlockerRectTransform.Maximize();

            return window;
        }

        private RectTransform GetOrCreateUIRoot()
        {
            if (_uiRootRectTransform == null)
            {
                GameObject instance = _instantiateService.Instantiate(_config.ContainerPrefab);
                instance.transform.SetParent(_lifetimeScope.transform);

                _uiRootRectTransform = (RectTransform)instance.transform;
                return instance.transform as RectTransform;
            }

            return _uiRootRectTransform;
        }
    }
}