﻿using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Popup.Core;
using Infrastructure.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Infrastructure.Services.Popup
{
    public class ConfirmationPopup : AnimatedPopup, IConfirmationPopup
    {
        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private readonly AutoResetCancellationTokenSource _inputCts = new AutoResetCancellationTokenSource();
        private readonly UniTaskCompletionSource<ConfirmationPopupResult> _resultTaskSource = new UniTaskCompletionSource<ConfirmationPopupResult>();

        private GameObject _previousSelectedGameObject;

        public UniTask<ConfirmationPopupResult> ResultTask => _resultTaskSource.Task;

        public override async UniTask Show()
        {
            _inputCts.Cancel();

            _previousSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(null);

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

            EventSystem.current.SetSelectedGameObject(_previousSelectedGameObject);

            await base.Hide();
        }

        private async UniTask<ConfirmationPopupResult> WaitUntilResult(CancellationToken token)
        {
            await UniTask.Yield(token).SuppressCancellationThrow();

            while (token.IsCancellationRequested == false)
            {
                if (_inputService.Actions.UI.Submit.WasPerformedThisFrame())
                {
                    await UniTask.Yield(token).SuppressCancellationThrow();
                    return ConfirmationPopupResult.Yes;
                }

                if (_inputService.Actions.UI.Cancel.WasPerformedThisFrame())
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