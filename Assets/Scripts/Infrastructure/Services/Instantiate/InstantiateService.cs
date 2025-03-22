using Cysharp.Threading.Tasks;
using Infrastructure.Services.Instantiate.Core;
using UnityEngine;

namespace Infrastructure.Services.Instantiate
{
    public class InstantiateService : IInstantiateService
    {
        public T Instantiate<T>(T prefab) where T : Object => Object.Instantiate(prefab);

        public async UniTask<T> InstantiateAsync<T>(T prefab) where T : Object
        {
            AsyncInstantiateOperation<T> instantiateOperation = Object.InstantiateAsync(prefab);

            await instantiateOperation.ToUniTask();

            return instantiateOperation.Result[0];
        }
    }
}