using Cysharp.Threading.Tasks;
using UniRx;

namespace Infrastructure.Services.Window.Core
{
    public interface IWindowService
    {
        public bool IsLoadingAnyWindow { get; }

        public IReadOnlyReactiveProperty<IWindow> TopWindow { get; }

        public UniTask<IWindow> CreateWindow(WindowID windowID);

        public UniTask<IWindow> GetOrCreateWindow(WindowID windowID);

        public bool TryFind(WindowID windowID, out IWindow window);
    }
}