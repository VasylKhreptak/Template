using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Asset.Core;
using Infrastructure.Services.Instantiate.Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

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

        public async UniTask<T> LoadAsync<T>(AssetReferenceT<T> assetReference, CancellationToken token = default) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);

            await handle.ToUniTask(cancellationToken: token).SuppressCancellationThrow();

            if (token.IsCancellationRequested)
            {
                handle.ReleaseHandleOnCompletion();
                throw new OperationCanceledException();
            }

            return handle.Result;
        }

        public void Release<T>(T asset) => Addressables.Release(asset);

        public async UniTask<T> InstantiateAsync<T>(AssetReferenceT<T> assetReference, CancellationToken token = default) where T : Component
        {
            T prefab = await LoadAsync<T>(assetReference, token);
            T instance = await _instantiateService.InstantiateAsync(prefab, token);
            instance.OnDestroyAsObservable().Subscribe(_ => Release(prefab)).AddTo(_releaseSubscriptions);
            return instance;
        }

        public UniTask<GameObject> InstantiateAsync(AssetReferenceT<GameObject> assetReference, CancellationToken token = default) =>
            InstantiateAsync(assetReference, null, token);

        public async UniTask<GameObject> InstantiateAsync(AssetReferenceT<GameObject> assetReference, Transform parent, CancellationToken token = default)
        {
            GameObject prefab = await LoadAsync<GameObject>(assetReference, token);
            GameObject instance = await _instantiateService.InstantiateAsync(prefab, parent, token);
            instance.OnDestroyAsObservable().Subscribe(_ => Release(prefab)).AddTo(_releaseSubscriptions);
            return instance;
        }

        public void Dispose() => _releaseSubscriptions?.Dispose();
    }
}