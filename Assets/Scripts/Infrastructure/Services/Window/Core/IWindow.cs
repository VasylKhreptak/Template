using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Services.Window.Core
{
    public interface IWindow
    {
        public RectTransform RootRectTransform { get; }

        public CanvasGroup RootCanvasGroup { get; }

        public UniTask Show();

        public UniTask Hide();
    }
}