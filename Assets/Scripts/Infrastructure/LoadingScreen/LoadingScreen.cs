using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Tools;
using UnityEngine;

namespace Infrastructure.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private readonly AutoResetCancellationTokenSource _cts = new AutoResetCancellationTokenSource();

        #region MonoBehaviour

        private void Awake() => DontDestroyOnLoad(gameObject);

        private void OnDestroy() => _cts.Cancel();

        #endregion

        public UniTask Show()
        {
            _cts.Cancel();
            _rectTransform.anchoredPosition = Vector2.zero;
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public async UniTask Hide()
        {
            if (gameObject.activeSelf == false)
                return;

            _cts.Cancel();
            await _rectTransform
                .DOAnchorPosY(_rectTransform.rect.height, _duration)
                .SetEase(_ease)
                .Play()
                .WithCancellation(_cts.Token)
                .SuppressCancellationThrow();

            gameObject.SetActive(false);
        }
    }
}