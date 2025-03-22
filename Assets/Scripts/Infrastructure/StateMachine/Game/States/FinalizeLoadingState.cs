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
        private readonly ILoadingScreen _loadingScreen;
        private readonly ILogService _logService;

        public FinalizeLoadingState(IStateMachine<IGameState> stateMachine, IStaticDataModel staticDataModel,
            ILoadingScreen loadingScreen, ILogService logService)
        {
            _stateMachine = stateMachine;
            _staticDataModel = staticDataModel;
            _loadingScreen = loadingScreen;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("FinalizeBootstrapState");

            LoadSceneAsyncState.Payload payload = new LoadSceneAsyncState.Payload
            {
                SceneName = _staticDataModel.Config.MenuScene,
                OnComplete = OnSceneLoaded
            };

            _stateMachine.Enter<LoadSceneAsyncState, LoadSceneAsyncState.Payload>(payload);
        }

        private void OnSceneLoaded() => _loadingScreen.Hide();
    }
}