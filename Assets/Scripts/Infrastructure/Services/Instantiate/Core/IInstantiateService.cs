using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Services.Instantiate.Core
{
    public interface IInstantiateService
    {
        public T Instantiate<T>(T prefab) where T : Object;

        public UniTask<T> InstantiateAsync<T>(T prefab) where T : Object;
    }
}