using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Tools;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Slider _progressSlider;

        [Header("Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private readonly AutoResetCancellationTokenSource _cts = new AutoResetCancellationTokenSource();

        private Progress<float> _currentProgress;

        #region MonoBehaviour

        private void OnValidate()
        {
            _canvasGroup ??= GetComponentInChildren<CanvasGroup>();
            _progressSlider ??= GetComponentInChildren<Slider>();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            HideInstantly();
        }

        private void OnDestroy() => _cts.Cancel();

        #endregion

        public UniTask Show()
        {
            _cts.Cancel();
            _progressSlider.value = 0f;
            gameObject.SetActive(true);
            return SetAlphaTask(1f, _cts.Token);
        }

        public async UniTask Hide()
        {
            _cts.Cancel();
            await SetAlphaTask(0f, _cts.Token);
            gameObject.SetActive(false);
        }

        public void ShowInstantly()
        {
            _cts.Cancel();
            _canvasGroup.alpha = 1f;
            _progressSlider.value = 0f;
            gameObject.SetActive(true);
        }

        public void HideInstantly()
        {
            _cts.Cancel();
            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public void AssignProgress(Progress<float> progress)
        {
            if (_currentProgress != null)
            {
                _currentProgress.ProgressChanged -= OnProgressChanged;
            }

            _currentProgress = progress;
            _currentProgress.ProgressChanged += OnProgressChanged;
        }

        private void OnProgressChanged(object sender, float value) => _progressSlider.value = value;

        private UniTask SetAlphaTask(float alpha, CancellationToken token) =>
            _canvasGroup
                .DOFade(alpha, _duration)
                .SetEase(_ease)
                .SetUpdate(true)
                .Play()
                .WithCancellation(token);
    }
}