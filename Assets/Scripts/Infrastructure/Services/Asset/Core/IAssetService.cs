using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.Asset.Core
{
    public interface IAssetService
    {
        public UniTask<T> LoadAsync<T>(AssetReference assetReference, CancellationToken token = default);

        public void Release<T>(T asset);

        public UniTask<T> InstantiateAsync<T>(AssetReferenceT<T> assetReference, CancellationToken token = default) where T : Component;
        public UniTask<GameObject> InstantiateAsync(AssetReferenceT<GameObject> assetReference, CancellationToken token = default);
        public UniTask<GameObject> InstantiateAsync(AssetReferenceT<GameObject> assetReference, Transform parent, CancellationToken token = default);
    }
}