using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using UnityEngine;

namespace Infrastructure.UI.Windows.Core
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseWindow : MonoBehaviour, IWindow
    {
        [Header("Base Window References")]
        [SerializeField] private RectTransform _rootRectTransform;
        [SerializeField] private CanvasGroup _rootCanvasGroup;

        public RectTransform RootRectTransform => _rootRectTransform;
        public CanvasGroup RootCanvasGroup => _rootCanvasGroup;

        #region MonoBehaivour

        protected virtual void OnValidate()
        {
            _rootRectTransform = GetComponent<RectTransform>();
            _rootCanvasGroup = GetComponent<CanvasGroup>();
        }

        #endregion

        public abstract UniTask Show();

        public abstract UniTask Hide();
    }
}