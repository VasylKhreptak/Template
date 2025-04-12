using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Instantiate.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.Instantiate
{
    public class InstantiateService : IInstantiateService
    {
        public T Instantiate<T>(T prefab) where T : Object => Object.Instantiate(prefab);
        public T Instantiate<T>(T prefab, Transform parent) where T : Object => Object.Instantiate(prefab, parent);

        public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object => Object.Instantiate(prefab, position, rotation);

        public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object =>
            Object.Instantiate(prefab, position, rotation, parent);

        public async UniTask<T> InstantiateAsync<T>(T prefab, CancellationToken token = default) where T : Object
        {
            AsyncInstantiateOperation<T> instantiateOperation = Object.InstantiateAsync(prefab);
            token.Register(() => instantiateOperation.Cancel());
            await instantiateOperation.ToUniTask(cancellationToken: token);
            return instantiateOperation.Result[0];
        }

        public async UniTask<T> InstantiateAsync<T>(T prefab, Transform parent, CancellationToken token = default) where T : Object
        {
            AsyncInstantiateOperation<T> instantiateOperation = Object.InstantiateAsync(prefab, parent);
            token.Register(() => instantiateOperation.Cancel());
            await instantiateOperation.ToUniTask(cancellationToken: token);
            return instantiateOperation.Result[0];
        }

        public async UniTask<T> InstantiateAsync<T>(T prefab, Vector3 position, Quaternion rotation, CancellationToken token = default) where T : Object
        {
            AsyncInstantiateOperation<T> instantiateOperation = Object.InstantiateAsync(prefab, position, rotation);
            token.Register(() => instantiateOperation.Cancel());
            await instantiateOperation.ToUniTask(cancellationToken: token);
            return instantiateOperation.Result[0];
        }

        public async UniTask<T> InstantiateAsync<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent, CancellationToken token = default) where T : Object
        {
            InstantiateParameters parameters = new InstantiateParameters
            {
                parent = parent
            };

            AsyncInstantiateOperation<T> instantiateOperation = Object.InstantiateAsync(prefab, position, rotation, parameters);
            token.Register(() => instantiateOperation.Cancel());
            await instantiateOperation.ToUniTask(cancellationToken: token);
            return instantiateOperation.Result[0];
        }
    }
}