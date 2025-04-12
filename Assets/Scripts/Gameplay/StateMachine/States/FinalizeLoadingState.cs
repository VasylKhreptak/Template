﻿using Cysharp.Threading.Tasks;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.UI.Windows.LoadingScreen.Core;

namespace Gameplay.StateMachine.States
{
    public class FinalizeLoadingState : IGameplayState, IState
    {
        private readonly IStateMachine<IGameplayState> _stateMachine;
        private readonly ILogService _logService;
        private readonly IInputService _inputService;
        private readonly IWindowService _windowService;

        public FinalizeLoadingState(IStateMachine<IGameplayState> stateMachine, ILogService logService, IInputService inputService, IWindowService windowService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _inputService = inputService;
            _windowService = windowService;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.FinalizeLoadingState.Enter");

            if (_windowService.TryFind(WindowID.LoadingScreen, out IWindow window) == false)
                return;

            ILoadingScreen loadingScreen = (ILoadingScreen)window;

            loadingScreen.Hide().ContinueWith(() => _inputService.SetActive(true)).Forget();
            _stateMachine.Enter<LoopState>();
        }
    }
}