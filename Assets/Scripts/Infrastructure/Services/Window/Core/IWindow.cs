using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Services.Window.Core
{
    public interface IWindow
    {
        public RectTransform RectTransform { get; }
        
        public CanvasGroup CanvasGroup { get; }

        public UniTask Show();

        public UniTask Hide();
    }
}