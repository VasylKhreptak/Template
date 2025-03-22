using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Tools;
using Infrastructure.UI.TransitionScreen.Core;
using UnityEngine;

namespace Infrastructure.UI.TransitionScreen
{
    public class TransitionScreen : MonoBehaviour, ITransitionScreen
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _curve;

        private readonly AutoResetCancellationTokenSource _cts = new AutoResetCancellationTokenSource();

        #region MonoBehaviour

        private void OnValidate() => _canvasGroup ??= GetComponent<CanvasGroup>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            HideImmediately();
        }

        private void OnDestroy() => _cts.Cancel();

        #endregion

        public UniTask Show()
        {
            gameObject.SetActive(true);
            return SetAlphaSmooth(1f);
        }

        public async UniTask Hide()
        {
            await SetAlphaSmooth(0);

            gameObject.SetActive(false);
        }

        public void ShowImmediately()
        {
            _cts.Cancel();
            _canvasGroup.alpha = 1;
            gameObject.SetActive(true);
        }

        public void HideImmediately()
        {
            _cts.Cancel();
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        private UniTask SetAlphaSmooth(float alpha)
        {
            _cts.Cancel();
            return _canvasGroup
                .DOFade(alpha, _duration)
                .SetEase(_curve)
                .Play()
                .WithCancellation(_cts.Token)
                .SuppressCancellationThrow();
        }
    }
}