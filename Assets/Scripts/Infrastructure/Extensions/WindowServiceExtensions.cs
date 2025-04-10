using Infrastructure.Services.Window.Core;

namespace Infrastructure.Extensions
{
    public static class WindowServiceExtensions
    {
        public static bool IsLoadingAnyWindowIncludingParent(this IWindowService windowService)
        {
            if (windowService.IsLoadingAnyWindow)
                return true;

            if (windowService.Parent == null)
                return false;

            return windowService.Parent.IsLoadingAnyWindowIncludingParent();
        }

        public static IWindow GetTopWindowIncludingParent(this IWindowService windowService)
        {
            if (windowService.GetTopWindow() != null)
                return windowService.GetTopWindow();

            if (windowService.Parent == null)
                return null;

            return windowService.Parent.GetTopWindowIncludingParent();
        }

        public static bool TryFindIncludingParent(this IWindowService windowService, WindowID windowID, out IWindow window)
        {
            if (windowService.TryFind(windowID, out window))
                return true;

            if (windowService.Parent == null)
            {
                window = null;
                return false;
            }

            return windowService.Parent.TryFindIncludingParent(windowID, out window);
        }

        public static IWindowService GetRootWindowService(this IWindowService windowService)
        {
            if (windowService.Parent == null)
                return windowService;

            return GetRootWindowService(windowService.Parent);
        }
    }
}