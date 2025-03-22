using Cysharp.Threading.Tasks;
using Gameplay.StateMachine.States.Core;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Input.Core;
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
        private readonly IInputService _inputService;

        public FinalizeLoadingState(IStateMachine<IGameplayState> stateMachine, ILogService logService, ILoadingScreen loadingScreen, IInputService inputService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
            _inputService = inputService;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.FinalizeLoadingState.Enter");
            _loadingScreen.Hide().ContinueWith(() => _inputService.SetActive(true)).Forget();
            _stateMachine.Enter<LoopState>();
        }
    }
}