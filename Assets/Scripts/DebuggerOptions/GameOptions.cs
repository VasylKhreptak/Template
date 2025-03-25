using System;
using System.ComponentModel;
using Cysharp.Threading.Tasks;
using DebuggerOptions.Core;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.Popup.Core;
using Infrastructure.Services.Vibration.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;

namespace DebuggerOptions
{
    public class GameOptions : BaseOptions
    {
        private const string Category = "Game";

        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILoadingScreen _loadingScreen;
        private readonly IVibrationService _vibrationService;
        private readonly ILogService _logService;
        private readonly IPopupService _popupService;

        public GameOptions(IStateMachine<IGameState> stateMachine, ILoadingScreen loadingScreen, IVibrationService vibrationService, ILogService logService,
            IPopupService popupService)
        {
            _stateMachine = stateMachine;
            _loadingScreen = loadingScreen;
            _vibrationService = vibrationService;
            _logService = logService;
            _popupService = popupService;
        }

        [Category(Category)]
        public void Reload() => _stateMachine.Enter<ReloadState>();

        [Category(Category)]
        public void VibrateHigh() => _vibrationService.Vibrate(VibrationPreset.High);

        [Category(Category)]
        public void VibrateMedium() => _vibrationService.Vibrate(VibrationPreset.Medium);

        [Category(Category)]
        public void ShowLoadingScreen() => _loadingScreen.Show().ContinueWith(() => _logService.Log("Show")).Forget();

        [Category(Category)]
        public void HideLoadingScreen() => _loadingScreen.Hide().ContinueWith(() => _logService.Log("Hide")).Forget();

        [Category(Category)]
        public void SaveData() => _stateMachine.Enter<SaveDataState, Action>(null);

        [Category(Category)]
        public void ShowTestContinuationPopup()
        {
            _popupService
                .Show(PopupID.TestContinuation)
                .ContinueWith(popup =>
                {
                    IContinuationPopup continuationPopup = (IContinuationPopup)popup;
                    continuationPopup.ContinueTask.ContinueWith(() => _logService.Log("Popup continue")).Forget();
                })
                .Forget();
        }

        [Category(Category)]
        public void ShowTestConfirmationPopup()
        {
            _popupService
                .Show(PopupID.TestConfirmation)
                .ContinueWith(popup =>
                {
                    IConfirmationPopup continuationPopup = (IConfirmationPopup)popup;
                    continuationPopup.ResultTask.ContinueWith(result => _logService.Log("Popup result: " + result)).Forget();
                })
                .Forget();
        }
    }
}