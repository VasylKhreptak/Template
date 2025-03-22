using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.AsyncScene.Core
{
    public interface IAsyncSceneService
    {
        public UniTask Load(string name);

        public UniTask LoadCurrent();
    }
}