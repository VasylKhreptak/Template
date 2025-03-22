using Infrastructure.Data.Models.Static.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Menu.StateMachine.States.Core;

namespace Menu.StateMachine.States
{
    public class LoadGameplayState : IMenuState, IState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ILogService _logService;
        private readonly IInputService _inputService;
        private readonly IStaticDataModel _staticDataModel;

        public LoadGameplayState(IStateMachine<IGameState> gameStateMachine, ILogService logService, IInputService inputService, IStaticDataModel staticDataModel)
        {
            _gameStateMachine = gameStateMachine;
            _logService = logService;
            _inputService = inputService;
            _staticDataModel = staticDataModel;
        }

        public void Enter()
        {
            _logService.Log("Menu.LoadGameplayState.Enter");

            _inputService.SetActive(false);

            LoadSceneAsyncState.Payload payload = new LoadSceneAsyncState.Payload
            {
                SceneName = _staticDataModel.Config.GameplayScene
            };

            _gameStateMachine.Enter<LoadSceneWithTransitionAsyncState, LoadSceneAsyncState.Payload>(payload);
        }
    }
}