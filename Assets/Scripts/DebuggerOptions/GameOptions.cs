using System.ComponentModel;
using DebuggerOptions.Core;
using Infrastructure.Services.Vibration.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.UI.TransitionScreen.Core;

namespace DebuggerOptions
{
    public class GameOptions : BaseOptions
    {
        private const string Category = "Game";

        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ITransitionScreen _transitionScreen;
        private readonly IVibrationService _vibrationService;

        public GameOptions(IStateMachine<IGameState> stateMachine, ITransitionScreen transitionScreen, IVibrationService vibrationService)
        {
            _stateMachine = stateMachine;
            _transitionScreen = transitionScreen;
            _vibrationService = vibrationService;
        }

        [Category(Category)]
        public void Reload() => _stateMachine.Enter<ReloadState>();

        [Category(Category)]
        public void ShowTransitionScreen() => _transitionScreen.Show();

        [Category(Category)]
        public void HideTransitionScreen() => _transitionScreen.Hide();

        [Category(Category)]
        public void VibrateHigh() => _vibrationService.Vibrate(VibrationPreset.High);

        [Category(Category)]
        public void VibrateMedium() => _vibrationService.Vibrate(VibrationPreset.Medium);
    }
}