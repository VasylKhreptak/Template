using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Input.Core;
using Infrastructure.Tools;
using Infrastructure.UI.Popups.Core;
using VContainer;

namespace Infrastructure.UI.Popups
{
    public class ConfirmationPopup : FadePopup, IConfirmationPopup
    {
        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private readonly AutoResetCancellationTokenSource _inputCts = new AutoResetCancellationTokenSource();
        private readonly UniTaskCompletionSource<ConfirmationPopupResult> _resultTaskSource = new UniTaskCompletionSource<ConfirmationPopupResult>();

        public UniTask<ConfirmationPopupResult> ResultTask => _resultTaskSource.Task;

        public override async UniTask Show()
        {
            _inputCts.Cancel();

            await base.Show();

            WaitUntilResult(_inputCts.Token)
                .ContinueWith(result =>
                {
                    _resultTaskSource.TrySetResult(result);
                    Hide().Forget();
                })
                .Forget();
        }

        public override async UniTask Hide()
        {
            _inputCts.Cancel();

            await base.Hide();
        }

        private async UniTask<ConfirmationPopupResult> WaitUntilResult(CancellationToken token)
        {
            await UniTask.Yield(token).SuppressCancellationThrow();

            while (token.IsCancellationRequested == false)
            {
                if (_inputService.UI.Submit.Value)
                {
                    await UniTask.Yield(token).SuppressCancellationThrow();
                    return ConfirmationPopupResult.Yes;
                }

                if (_inputService.UI.Cancel.Value)
                {
                    await UniTask.Yield(token).SuppressCancellationThrow();
                    return ConfirmationPopupResult.No;
                }

                await UniTask.Yield(token).SuppressCancellationThrow();
            }

            return ConfirmationPopupResult.No;
        }
    }
}