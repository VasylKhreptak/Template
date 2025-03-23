using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Data.Models.Static.Core;
using Infrastructure.LoadingScreen.Core;
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
        private readonly ILoadingScreen _loadingScreen;

        public LoadGameplayState(IStateMachine<IGameState> gameStateMachine, ILogService logService, IInputService inputService, IStaticDataModel staticDataModel,
            ILoadingScreen loadingScreen)
        {
            _gameStateMachine = gameStateMachine;
            _logService = logService;
            _inputService = inputService;
            _staticDataModel = staticDataModel;
            _loadingScreen = loadingScreen;
        }

        public void Enter()
        {
            _logService.Log("Menu.LoadGameplayState.Enter");

            _inputService.SetActive(false);

            _loadingScreen
                .Show()
                .ContinueWith(() =>
                {
                    Progress<float> progress = new Progress<float>(x => _loadingScreen.SetProgress(x));

                    LoadSceneState.Payload payload = new LoadSceneState.Payload
                    {
                        SceneName = _staticDataModel.Config.GameplayScene,
                        Progress = progress
                    };

                    _gameStateMachine.Enter<LoadSceneState, LoadSceneState.Payload>(payload);
                })
                .Forget();
        }
    }
}