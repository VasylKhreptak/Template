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

        public IWindow TopWindow => _windows.Last.Value.Window;

        public async UniTask<IWindow> CreateWindow(WindowID windowID)
        {
            GameObject previousSelectedGameObject = EventSystem.current.currentSelectedGameObject;

            EventSystem.current.SetSelectedGameObject(null);

            IWindow window = await _windowFactory.CreateWindow(windowID);

            WindowInfo info = new WindowInfo
            {
                ID = windowID,
                Window = window,
                PreviousSelectedGameObject = previousSelectedGameObject,
                DestroySubscription = window.RectTransform.OnDestroyAsObservable().Subscribe(_ => OnBeforeWindowDestroy(window))
            };

            _windows.AddLast(info);

            return window;
        }

        public bool TryFindFirst(WindowID windowID, out IWindow window)
        {
            foreach (WindowInfo windowInfo in _windows)
            {
                if (windowInfo.ID == windowID)
                {
                    window = windowInfo.Window;
                    return false;
                }
            }

            window = null;
            return false;
        }

        public bool TryFindLast(WindowID windowID, out IWindow window)
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

                if (node.Value.Window == window)
                {
                    if (node == lastNode)
                        EventSystem.current.SetSelectedGameObject(node.Value.PreviousSelectedGameObject);
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