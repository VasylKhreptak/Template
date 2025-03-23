using Cysharp.Threading.Tasks;

namespace Infrastructure.LoadingScreen.Core
{
    public interface ILoadingScreen
    {
        public UniTask Show();

        public UniTask Hide();

        public void ShowInstantly();

        public void HideInstantly();

        public void SetProgress(float progress);
    }
}