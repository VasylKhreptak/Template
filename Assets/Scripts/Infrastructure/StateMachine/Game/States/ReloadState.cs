using System;
using Cysharp.Threading.Tasks;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class ReloadState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly ILoadingScreen _loadingScreen;
        private readonly IInputService _inputService;

        public ReloadState(IStateMachine<IGameState> stateMachine, ILogService logService, ILoadingScreen loadingScreen, IInputService inputService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
            _inputService = inputService;
        }

        public void Enter()
        {
            _logService.Log("Game.ReloadState.Enter");

            _inputService.SetActive(false);

            _loadingScreen
                .Show()
                .ContinueWith(() => _stateMachine.Enter<SaveDataState, Action>(() => _stateMachine.Enter<BootstrapState>()))
                .Forget();
        }
    }
}