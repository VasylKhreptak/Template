using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Menu.StateMachine.States.Core;

namespace Menu.StateMachine.States
{
    public class BootstrapState : IMenuState, IState
    {
        private readonly IStateMachine<IMenuState> _stateMachine;
        private readonly ILogService _logService;

        public BootstrapState(IStateMachine<IMenuState> stateMachine, ILogService logService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("Menu.BootstrapState.Enter");
            _stateMachine.Enter<FinalizeLoadingState>();
        }
    }
}