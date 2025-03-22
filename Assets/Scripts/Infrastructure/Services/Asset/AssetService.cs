using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Asset.Core;
using Infrastructure.Services.Instantiate.Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.Asset
{
    public class AssetService : IAssetService, IDisposable
    {
        private readonly IInstantiateService _instantiateService;

        public AssetService(IInstantiateService instantiateService)
        {
            _instantiateService = instantiateService;
        }

        private readonly CompositeDisposable _releaseSubscriptions = new CompositeDisposable();

        public UniTask<T> LoadAsync<T>(AssetReference assetReference) => Addressables.LoadAssetAsync<T>(assetReference).ToUniTask();

        public void Release<T>(T asset) => Addressables.Release(asset);

        public async UniTask<GameObject> InstantiateAsync(AssetReferenceGameObject assetReference)
        {
            GameObject prefab = await LoadAsync<GameObject>(assetReference);

            GameObject instance = await _instantiateService.InstantiateAsync(prefab);

            instance.OnDestroyAsObservable().Subscribe(_ => Release(prefab)).AddTo(_releaseSubscriptions);

            return instance;
        }

        public void Dispose() => _releaseSubscriptions?.Dispose();
    }
}