using Cysharp.Threading.Tasks;
using UniRx;

namespace Infrastructure.Services.Window.Core
{
    public interface IWindowService
    {
        public IReadOnlyReactiveProperty<IWindow> TopWindow { get; }

        public bool IsLoadingAnyWindow { get; }

        public UniTask<IWindow> CreateWindow(WindowID windowID);

        public bool TryFindFirst(WindowID windowID, out IWindow window);

        public bool TryFindLast(WindowID windowID, out IWindow window);
    }
}