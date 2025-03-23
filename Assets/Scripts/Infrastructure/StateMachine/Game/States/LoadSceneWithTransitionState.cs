using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.UI.TransitionScreen.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadSceneWithTransitionState : IGameState, IPayloadedState<LoadSceneWithTransitionState.Payload>
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ITransitionScreen _transitionScreen;
        private readonly ILogService _logService;

        public LoadSceneWithTransitionState(IStateMachine<IGameState> stateMachine, ITransitionScreen transitionScreen,
            ILogService logService)
        {
            _stateMachine = stateMachine;
            _transitionScreen = transitionScreen;
            _logService = logService;
        }

        public void Enter(Payload payload)
        {
            _logService.Log($"Game.LoadSceneWithTransitionAsyncState.Enter: {payload.SceneName}");

            _transitionScreen
                .Show()
                .ContinueWith(() =>
                {
                    payload.OnAfterTransitionScreenShow?.Invoke();

                    payload.OnComplete += () =>
                    {
                        payload.OnBeforeTransitionScreenHide?.Invoke();
                        _transitionScreen.Hide();
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