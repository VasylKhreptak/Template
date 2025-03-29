using System.Threading;
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
        [Header("References")]
        [SerializeField] private CanvasGroup _contentCanvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutCubic;

        private readonly AutoResetCancellationTokenSource _cts = new AutoResetCancellationTokenSource();

        #region MonoBehaviour

        protected override void OnValidate()
        {
            base.OnValidate();

            _contentCanvasGroup ??= GetComponent<CanvasGroup>();
        }

        protected virtual void Awake() => Disable();

        protected virtual void OnDestroy() => _cts.Cancel();

        #endregion

        public override async UniTask Show()
        {
            _cts.Cancel();
            _contentCanvasGroup.gameObject.SetActive(true);
            await SetAlphaTask(1f, _cts.Token);
            _contentCanvasGroup.interactable = true;
        }

        public override async UniTask Hide()
        {
            _cts.Cancel();
            _contentCanvasGroup.interactable = false;
            await SetAlphaTask(0f, _cts.Token);
            Destroy(gameObject);
        }

        private UniTask SetAlphaTask(float alpha, CancellationToken token) =>
            _contentCanvasGroup
                .DOFade(alpha, _duration)
                .SetEase(_ease)
                .SetUpdate(true)
                .Play()
                .WithCancellation(token);

        private void Disable()
        {
            _contentCanvasGroup.alpha = 0f;
            _contentCanvasGroup.gameObject.SetActive(false);
        }
    }
}