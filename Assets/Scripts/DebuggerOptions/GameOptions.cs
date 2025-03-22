using System.ComponentModel;
using DebuggerOptions.Core;
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

        public GameOptions(IStateMachine<IGameState> stateMachine, ITransitionScreen transitionScreen)
        {
            _stateMachine = stateMachine;
            _transitionScreen = transitionScreen;
        }

        [Category(Category)]
        public void Reload() => _stateMachine.Enter<ReloadState>();

        [Category(Category)]
        public void ShowTransitionScreen() => _transitionScreen.Show();

        [Category(Category)]
        public void HideTransitionScreen() => _transitionScreen.Hide();
    }
}