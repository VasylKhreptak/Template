﻿using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.Asset.Core
{
    public interface IAssetService
    {
        public UniTask<T> LoadAsync<T>(AssetReference assetReference);

        public void Release<T>(T asset);

        public UniTask<T> InstantiateAsync<T>(AssetReferenceT<T> assetReference) where T : Component;
        public UniTask<GameObject> InstantiateAsync(AssetReferenceT<GameObject> assetReference);
    }
}