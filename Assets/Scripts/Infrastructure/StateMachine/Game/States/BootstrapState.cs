using Cysharp.Threading.Tasks;
using Infrastructure.Data.Models.Static.Core;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.AsyncScene.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILoadingScreen _loadingScreen;
        private readonly ILogService _logService;
        private readonly IAsyncSceneService _sceneService;
        private readonly IStaticDataModel _staticDataModel;

        public BootstrapState(IStateMachine<IGameState> stateMachine, ILoadingScreen loadingScreen, ILogService logService,
            IAsyncSceneService sceneService, IStaticDataModel staticDataModel)
        {
            _stateMachine = stateMachine;
            _loadingScreen = loadingScreen;
            _logService = logService;
            _sceneService = sceneService;
            _staticDataModel = staticDataModel;
        }

        public async void Enter()
        {
            _logService.Log("BootstrapState");

            _loadingScreen.Show().Forget();

            await _sceneService.Load(_staticDataModel.Config.BootstrapScene);

            _stateMachine.Enter<LoadDataState>();
        }
    }
}