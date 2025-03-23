using Cysharp.Threading.Tasks;

namespace Infrastructure.LoadingScreen.Core
{
    public interface ILoadingScreen
    {
        public UniTask Show();

        public UniTask Hide();

        public void ShowInstant();

        public void HideInstant();
    }
}