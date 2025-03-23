using System;
using Infrastructure.Data.Models.Static.Core;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class FinalizeLoadingState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IStaticDataModel _staticDataModel;
        private readonly ILogService _logService;
        private readonly ILoadingScreen _loadingScreen;

        public FinalizeLoadingState(IStateMachine<IGameState> stateMachine, IStaticDataModel staticDataModel, ILogService logService, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _staticDataModel = staticDataModel;
            _logService = logService;
            _loadingScreen = loadingScreen;
        }

        public void Enter()
        {
            _logService.Log("Game.FinalizeLoadingState.Enter");

            Progress<float> progress = new Progress<float>(x => _loadingScreen.SetProgress(x));

            LoadSceneState.Payload payload = new LoadSceneState.Payload
            {
                SceneName = _staticDataModel.Config.MenuScene,
                Progress = progress
            };

            _stateMachine.Enter<LoadSceneState, LoadSceneState.Payload>(payload);
        }
    }
}