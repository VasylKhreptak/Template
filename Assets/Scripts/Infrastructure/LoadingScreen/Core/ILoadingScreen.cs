using Infrastructure.Services.Window.Core;

namespace Infrastructure.LoadingScreen.Core
{
    public interface ILoadingScreen : IWindow
    {
        public void ShowInstantly();

        public void HideInstantly();

        public void SetProgress(float progress);
    }
}