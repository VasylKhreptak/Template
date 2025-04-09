using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [Header("References")]
        [SerializeField] private RectTransform _rootRectTransform;
        [SerializeField] private CanvasGroup _rootCanvasGroup;
        [SerializeField] private Slider _progressSlider;

        [Header("Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private readonly AutoResetCancellationTokenSource _cts = new AutoResetCancellationTokenSource();

        public RectTransform RootRectTransform => _rootRectTransform;
        public CanvasGroup RootCanvasGroup => _rootCanvasGroup;

        #region MonoBehaviour

        private void OnValidate()
        {
            _rootCanvasGroup ??= GetComponentInChildren<CanvasGroup>();
            _progressSlider ??= GetComponentInChildren<Slider>();
        }

        private void Awake() => Disable();

        private void OnDestroy() => _cts.Cancel();

        #endregion

        public UniTask Show()
        {
            _cts.Cancel();
            SetProgress(0f);
            gameObject.SetActive(true);
            return SetAlphaTask(1f, _cts.Token);
        }

        public UniTask Hide()
        {
            _cts.Cancel();
            return SetAlphaTask(0f, _cts.Token).ContinueWith(() => Destroy(gameObject));
        }

        public void ShowInstantly()
        {
            _cts.Cancel();
            _rootCanvasGroup.alpha = 1f;
            SetProgress(0f);
            gameObject.SetActive(true);
        }

        public void HideInstantly()
        {
            _cts.Cancel();
            _rootCanvasGroup.alpha = 0f;
            Destroy(gameObject);
        }

        public void SetProgress(float progress) => _progressSlider.value = progress;

        private void Disable()
        {
            _cts.Cancel();
            _rootCanvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        private UniTask SetAlphaTask(float alpha, CancellationToken token) =>
            _rootCanvasGroup
                .DOFade(alpha, _duration)
                .SetEase(_ease)
                .SetUpdate(true)
                .Play()
                .WithCancellation(token);
    }
}