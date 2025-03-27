using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Tools;
using Infrastructure.UI.Popups.Core;
using UnityEngine;

namespace Infrastructure.UI.Popups
{
    public class AnimatedPopup : MonoBehaviour, IPopup
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutCubic;

        private readonly AutoResetCancellationTokenSource _cts = new AutoResetCancellationTokenSource();

        public RectTransform RootRectTransform => _rectTransform;

        public CanvasGroup RootCanvasGroup => _canvasGroup;

        #region MonoBehaviour

        protected virtual void OnValidate()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
            _rectTransform ??= GetComponent<RectTransform>();
        }

        protected virtual void Awake() => Disable();

        protected virtual void OnDestroy() => _cts.Cancel();

        #endregion

        public virtual UniTask Show()
        {
            _cts.Cancel();
            _canvasGroup.gameObject.SetActive(true);
            return SetAlphaTask(1f, _cts.Token);
        }

        public virtual async UniTask Hide()
        {
            _cts.Cancel();
            await SetAlphaTask(0f, _cts.Token);
            Destroy(gameObject);
        }

        private UniTask SetAlphaTask(float alpha, CancellationToken token) =>
            _canvasGroup
                .DOFade(alpha, _duration)
                .SetEase(_ease)
                .SetUpdate(true)
                .Play()
                .WithCancellation(token);

        private void Disable()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.gameObject.SetActive(false);
        }
    }
}