using Cysharp.Threading.Tasks;

namespace Infrastructure.UI.TransitionScreen.Core
{
    public interface ITransitionScreen
    {
        public UniTask Show();

        public UniTask Hide();

        public void ShowImmediately();

        public void HideImmediately();
    }
}