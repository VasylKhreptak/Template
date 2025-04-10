using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Window.Core
{
    public interface IWindowService
    {
        public IWindowService Parent { get; }

        public bool IsLoadingAnyWindow { get; }

        public IWindow GetTopWindow();

        public UniTask<IWindow> CreateWindow(WindowID windowID);

        public UniTask<IWindow> GetOrCreateWindow(WindowID windowID);

        public bool TryFind(WindowID windowID, out IWindow window);
    }
}