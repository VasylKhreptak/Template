﻿using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Tools;
using Infrastructure.UI.Popups.Core;
using Infrastructure.UI.Windows.Core;
using UnityEngine;

namespace Infrastructure.UI.Popups
{
    public class FadePopup : BaseWindow, IPopup
    {
        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutCubic;

        private readonly AutoResetCancellationTokenSource _cts = new AutoResetCancellationTokenSource();

        #region MonoBehaviour

        protected virtual void Awake() => Disable();

        protected virtual void OnDestroy() => _cts.Cancel();

        #endregion

        public override async UniTask Show()
        {
            _cts.Cancel();
            RootCanvasGroup.gameObject.SetActive(true);
            await SetAlphaTask(1f, _cts.Token);
            RootCanvasGroup.interactable = true;
        }

        public override async UniTask Hide()
        {
            _cts.Cancel();
            RootCanvasGroup.interactable = false;
            await SetAlphaTask(0f, _cts.Token);
            Destroy(gameObject);
        }

        private UniTask SetAlphaTask(float alpha, CancellationToken token) =>
            RootCanvasGroup
                .DOFade(alpha, _duration)
                .SetEase(_ease)
                .SetUpdate(true)
                .Play()
                .WithCancellation(token);

        private void Disable()
        {
            RootCanvasGroup.alpha = 0f;
            RootCanvasGroup.gameObject.SetActive(false);
            RootCanvasGroup.interactable = false;
        }
    }
}