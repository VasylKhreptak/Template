using System;
using Cysharp.Threading.Tasks;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Data.Models.Static.Core;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Input.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.Popup.Core;
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
        private readonly ILoadingScreen _loadingScreen;
        private readonly IPopupService _popupService;

        public LoadMenuState(IStateMachine<IGameplayState> gameplayStateMachine, IStateMachine<IGameState> gameStateMachine, ILogService logService,
            IInputService inputService, IStaticDataModel staticDataModel, ILoadingScreen loadingScreen, IPopupService popupService)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _gameStateMachine = gameStateMachine;
            _logService = logService;
            _inputService = inputService;
            _staticDataModel = staticDataModel;
            _loadingScreen = loadingScreen;
            _popupService = popupService;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.LoadMenuState.Enter");

            _inputService.SetActive(false);

            _loadingScreen
                .Show()
                .ContinueWith(() =>
                {
                    _popupService.DestroyAll();

                    Progress<float> progress = new Progress<float>(x => _loadingScreen.SetProgress(x));

                    _gameplayStateMachine.Enter<SaveDataState, Action>(null);

                    LoadSceneState.Payload payload = new LoadSceneState.Payload
                    {
                        SceneName = _staticDataModel.Config.MenuScene,
                        Progress = progress
                    };

                    _gameStateMachine.Enter<LoadSceneState, LoadSceneState.Payload>(payload);
                })
                .Forget();
        }
    }
}