using System;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.UI.TransitionScreen.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class ReloadState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly ITransitionScreen _transitionScreen;

        public ReloadState(IStateMachine<IGameState> stateMachine, ILogService logService, ITransitionScreen transitionScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _transitionScreen = transitionScreen;
        }

        public async void Enter()
        {
            _logService.Log("ReloadState");

            await _transitionScreen.Show();

            _stateMachine.Enter<SaveDataState, Action>(() =>
            {
                _stateMachine.Enter<BootstrapState>();
                _transitionScreen.HideImmediately();
            });
        }
    }
}