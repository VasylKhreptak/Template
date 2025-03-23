using System;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Data.Models.Static.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Gameplay.StateMachine.States
{
    public class LoadMenuState : IGameplayState, IState
    {
        private readonly IStateMachine<IGameplayState> _gameplayStateMachine;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ILogService _logService;
        private readonly IInputService _inputService;
        private readonly IStaticDataModel _staticDataModel;

        public LoadMenuState(IStateMachine<IGameplayState> gameplayStateMachine, IStateMachine<IGameState> gameStateMachine, ILogService logService,
            IInputService inputService, IStaticDataModel staticDataModel)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _gameStateMachine = gameStateMachine;
            _logService = logService;
            _inputService = inputService;
            _staticDataModel = staticDataModel;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.LoadMenuState.Enter");

            _inputService.SetActive(false);

            LoadSceneWithTransitionState.Payload payload = new LoadSceneWithTransitionState.Payload()
            {
                SceneName = _staticDataModel.Config.MenuScene,
                OnAfterTransitionScreenShow = SaveData
            };

            _gameStateMachine.Enter<LoadSceneWithTransitionState, LoadSceneWithTransitionState.Payload>(payload);
        }

        private void SaveData() => _gameplayStateMachine.Enter<SaveDataState, Action>(null);
    }
}