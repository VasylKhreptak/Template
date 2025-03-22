using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Instantiate.Core;
using UnityEngine;

namespace Infrastructure.Services.Instantiate
{
    public class InstantiateService : IInstantiateService
    {
        public T Instantiate<T>(T prefab) where T : Object => Object.Instantiate(prefab);

        public async UniTask<T> InstantiateAsync<T>(T prefab, CancellationToken token = default) where T : Object
        {
            AsyncInstantiateOperation<T> instantiateOperation = Object.InstantiateAsync(prefab);

            await instantiateOperation.ToUniTask(cancellationToken: token);

            return instantiateOperation.Result[0];
        }
    }
}