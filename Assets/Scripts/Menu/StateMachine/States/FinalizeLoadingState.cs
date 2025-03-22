using Cysharp.Threading.Tasks;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Menu.StateMachine.States.Core;

namespace Menu.StateMachine.States
{
    public class FinalizeLoadingState : IMenuState, IState
    {
        private readonly IStateMachine<IMenuState> _stateMachine;
        private readonly ILogService _logService;
        private readonly ILoadingScreen _loadingScreen;
        private readonly IInputService _inputService;

        public FinalizeLoadingState(IStateMachine<IMenuState> stateMachine, ILogService logService, ILoadingScreen loadingScreen, IInputService inputService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
            _inputService = inputService;
        }

        public void Enter()
        {
            _logService.Log("Menu.FinalizeLoadingState.Enter");
            _loadingScreen.Hide().ContinueWith(() => _inputService.SetActive(true)).Forget();
            _stateMachine.Enter<LoopState>();
        }
    }
}