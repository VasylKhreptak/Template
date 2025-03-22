using Cysharp.Threading.Tasks;
using Infrastructure.Services.AsyncScene.Core;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.AsyncScene
{
    public class AsyncSceneService : IAsyncSceneService
    {
        public async UniTask Load(string name) => await SceneManager.LoadSceneAsync(name);

        public UniTask LoadCurrent() => Load(SceneManager.GetActiveScene().name);
    }
}