using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using Infrastructure.Services.Window.Factories.Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Infrastructure.Services.Window
{
    public class WindowService : IWindowService
    {
        private readonly IWindowFactory _windowFactory;

        public WindowService(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        private readonly LinkedList<WindowInfo> _windows = new LinkedList<WindowInfo>();

        private readonly ReactiveProperty<IWindow> _topWindow = new ReactiveProperty<IWindow>();

        public IReadOnlyReactiveProperty<IWindow> TopWindow => _topWindow;

        public bool IsLoadingAnyWindow { get; private set; }

        public async UniTask<IWindow> CreateWindow(WindowID windowID)
        {
            IsLoadingAnyWindow = true;

            GameObject previousSelectedGameObject = EventSystem.current.currentSelectedGameObject;

            EventSystem.current.SetSelectedGameObject(null);

            IWindow window = await _windowFactory.CreateWindow(windowID);

            WindowInfo info = new WindowInfo
            {
                ID = windowID,
                Window = window,
                PreviousSelectedGameObject = previousSelectedGameObject,
                DestroySubscription = window.RootRectTransform.OnDestroyAsObservable().Subscribe(_ => OnBeforeWindowDestroy(window))
            };

            IWindow topWindow = GetTopWindow();

            if (topWindow != null)
                topWindow.RootCanvasGroup.interactable = false;

            window.RootCanvasGroup.interactable = false;

            _windows.AddLast(info);
            _topWindow.Value = info.Window;

            IsLoadingAnyWindow = false;

            return window;
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
                    EventSystem.current?.SetSelectedGameObject(node.Value.PreviousSelectedGameObject);

                    LinkedListNode<WindowInfo> previousNode = node.Previous;

                    if (previousNode != null)
                    {
                        previousNode.Value.Window.RootCanvasGroup.interactable = true;
                        _topWindow.Value = previousNode.Value.Window;
                    }
                    else
                        _topWindow.Value = null;
                }
                else
                {
                    LinkedListNode<WindowInfo> nextNode = node.Next;

                    if (nextNode != null)
                        nextNode.Value.PreviousSelectedGameObject = node.Value.PreviousSelectedGameObject;
                }

                windowInfo.DestroySubscription.Dispose();
                _windows.Remove(windowInfo);
                return;
            }
        }

        private IWindow GetTopWindow()
        {
            if (_windows.Count == 0)
                return null;

            return _windows.Last.Value.Window;
        }

        private class WindowInfo
        {
            public WindowID ID;
            public IWindow Window;
            public GameObject PreviousSelectedGameObject;
            public IDisposable DestroySubscription;
        }
    }
}