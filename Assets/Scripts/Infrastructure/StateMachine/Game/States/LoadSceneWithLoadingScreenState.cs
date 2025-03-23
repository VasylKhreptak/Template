using System;
using Cysharp.Threading.Tasks;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadSceneWithLoadingScreenState : IGameState, IPayloadedState<LoadSceneWithLoadingScreenState.Payload>
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILoadingScreen _loadingScreen;
        private readonly ILogService _logService;

        public LoadSceneWithLoadingScreenState(IStateMachine<IGameState> stateMachine, ILoadingScreen loadingScreen,
            ILogService logService)
        {
            _stateMachine = stateMachine;
            _loadingScreen = loadingScreen;
            _logService = logService;
        }

        public void Enter(Payload payload)
        {
            _logService.Log($"Game.LoadSceneWithLoadingScreenAsyncState.Enter: {payload.SceneName}");

            _loadingScreen
                .Show()
                .ContinueWith(() =>
                {
                    payload.OnAfterTransitionScreenShow?.Invoke();

                    payload.OnComplete += () =>
                    {
                        payload.OnBeforeTransitionScreenHide?.Invoke();
                        _loadingScreen.Hide();
                    };

                    _stateMachine.Enter<LoadSceneState, LoadSceneState.Payload>(payload);
                })
                .Forget();
        }

        public class Payload : LoadSceneState.Payload
        {
            public Action OnAfterTransitionScreenShow;
            public Action OnBeforeTransitionScreenHide;
        }
    }
}