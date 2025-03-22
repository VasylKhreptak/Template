using Infrastructure.Data.Models.Persistent.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapAnalyticsState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IPersistentDataModel _persistentDataModel;
        private readonly ILogService _logService;

        public BootstrapAnalyticsState(IStateMachine<IGameState> gameStateMachine, IPersistentDataModel persistentDataModel,
            ILogService logService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataModel = persistentDataModel;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("BootstrapAnalyticsState");
            _persistentDataModel.Data.AnalyticsData.SessionsCount++;
            _gameStateMachine.Enter<FinalizeBootstrapState>();
        }
    }
}