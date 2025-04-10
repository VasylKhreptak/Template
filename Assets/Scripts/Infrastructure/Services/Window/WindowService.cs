using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using Infrastructure.Services.Window.Core.EventHandlers;
using Infrastructure.Services.Window.Factories.Core;
using UniRx;
using UniRx.Triggers;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Services.Window
{
    public class WindowService : IWindowService
    {
        private readonly IWindowFactory _windowFactory;

        public WindowService(IWindowFactory windowFactory, LifetimeScope scope)
        {
            _windowFactory = windowFactory;

            if (scope.Parent != null)
                if (scope.Parent.Container.TryResolve(out IWindowService parentWindowService))
                    Parent = parentWindowService;
        }

        private readonly LinkedList<WindowInfo> _windows = new LinkedList<WindowInfo>();

        public IWindowService Parent { get; }

        public bool IsLoadingAnyWindow { get; private set; }

        public async UniTask<IWindow> CreateWindow(WindowID windowID)
        {
            IsLoadingAnyWindow = true;

            IWindow topWindow = GetTopWindow();

            if (topWindow is IWindowInactiveEventHandler previousWindowInactiveEventHandler)
                previousWindowInactiveEventHandler.OnBecameInactive();

            IWindow window = await _windowFactory.CreateWindow(windowID);

            WindowInfo info = new WindowInfo
            {
                ID = windowID,
                Window = window,
                DestroySubscription = window.RootRectTransform.OnDestroyAsObservable().Subscribe(_ => OnBeforeWindowDestroy(window))
            };

            _windows.AddLast(info);

            if (window is IWindowActiveEventHandler windowActiveEventHandler)
                windowActiveEventHandler.OnBecameActive();

            IsLoadingAnyWindow = false;

            return window;
        }

        public UniTask<IWindow> GetOrCreateWindow(WindowID windowID)
        {
            if (TryFind(windowID, out IWindow window))
                return UniTask.FromResult(window);

            return CreateWindow(windowID);
        }

        public bool TryFind(WindowID windowID, out IWindow window)
        {
            for (LinkedListNode<WindowInfo> node = _windows.Last; node != null; node = node.Previous)
            {
                if (node.Value.ID == windowID)
                {
                    window = node.Value.Window;
                    return true;
                }
            }

            window = null;
            return false;
        }

        private void OnBeforeWindowDestroy(IWindow window)
        {
            LinkedListNode<WindowInfo> lastNode = _windows.Last;

            for (LinkedListNode<WindowInfo> node = _windows.Last; node != null; node = node.Previous)
            {
                WindowInfo windowInfo = node.Value;

                if (node.Value.Window != window)
                    continue;

                if (node == lastNode)
                {
                    LinkedListNode<WindowInfo> previousNode = node.Previous;

                    if (window is IWindowInactiveEventHandler inactiveEventHandler)
                        inactiveEventHandler.OnBecameInactive();

                    if (previousNode != null)
                    {
                        if (previousNode.Value.Window is IWindowActiveEventHandler activeEventHandler)
                            activeEventHandler.OnBecameActive();
                    }
                }

                windowInfo.DestroySubscription.Dispose();
                _windows.Remove(windowInfo);
                return;
            }
        }

        public IWindow GetTopWindow()
        {
            if (_windows.Count == 0)
                return null;

            return _windows.Last.Value.Window;
        }

        private class WindowInfo
        {
            public WindowID ID;
            public IWindow Window;
            public IDisposable DestroySubscription;
        }
    }
}