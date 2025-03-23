using System.ComponentModel;
using Cysharp.Threading.Tasks;
using DebuggerOptions.Core;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
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

        public GameOptions(IStateMachine<IGameState> stateMachine, ILoadingScreen loadingScreen, IVibrationService vibrationService, ILogService logService)
        {
            _stateMachine = stateMachine;
            _loadingScreen = loadingScreen;
            _vibrationService = vibrationService;
            _logService = logService;
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
    }
}