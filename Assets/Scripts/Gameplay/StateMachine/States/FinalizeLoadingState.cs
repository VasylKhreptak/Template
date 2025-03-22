using Gameplay.StateMachine.States.Core;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Gameplay.StateMachine.States
{
    public class FinalizeLoadingState : IGameplayState, IState
    {
        private readonly IStateMachine<IGameplayState> _stateMachine;
        private readonly ILogService _logService;
        private readonly ILoadingScreen _loadingScreen;

        public FinalizeLoadingState(IStateMachine<IGameplayState> stateMachine, ILogService logService, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.FinalizeLoadingState.Enter");
            _loadingScreen.Hide();
            _stateMachine.Enter<LoopState>();
        }
    }
}