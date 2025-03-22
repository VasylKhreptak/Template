using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.Asset.Core
{
    public interface IAssetService
    {
        public UniTask<T> LoadAsync<T>(AssetReference assetReference);

        public void Release<T>(T asset);

        public UniTask<GameObject> InstantiateAsync(AssetReferenceGameObject assetReference);
    }
}