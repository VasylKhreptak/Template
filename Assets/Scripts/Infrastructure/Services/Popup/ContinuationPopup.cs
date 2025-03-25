using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Popup.Core;
using Infrastructure.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Infrastructure.Services.Popup
{
    public class ContinuationPopup : AnimatedPopup, IContinuationPopup
    {
        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private readonly AutoResetCancellationTokenSource _inputCts = new AutoResetCancellationTokenSource();
        private readonly UniTaskCompletionSource _continuationTaskSource = new UniTaskCompletionSource();

        private GameObject _previousSelectedGameObject;

        public UniTask ContinueTask => _continuationTaskSource.Task;

        public override async UniTask Show()
        {
            _inputCts.Cancel();

            _previousSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(null);

            await base.Show();

            WaitUntilContinue(_inputCts.Token)
                .ContinueWith(() =>
                {
                    _continuationTaskSource.TrySetResult();
                    Hide().Forget();
                })
                .Forget();
        }

        public override async UniTask Hide()
        {
            _inputCts.Cancel();

            await base.Hide();

            EventSystem.current.SetSelectedGameObject(_previousSelectedGameObject);
        }

        private async UniTask WaitUntilContinue(CancellationToken token)
        {
            await UniTask.Yield(token).SuppressCancellationThrow();

            while (token.IsCancellationRequested == false)
            {
                if (_inputService.Actions.UI.Submit.WasPerformedThisFrame())
                {
                    await UniTask.Yield(token).SuppressCancellationThrow();
                    return;
                }

                await UniTask.Yield(token).SuppressCancellationThrow();
            }
        }
    }
}