using Infrastructure.Data.Models.Static.Core;
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

        public FinalizeLoadingState(IStateMachine<IGameState> stateMachine, IStaticDataModel staticDataModel, ILogService logService)
        {
            _stateMachine = stateMachine;
            _staticDataModel = staticDataModel;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("Game.FinalizeLoadingState.Enter");

            LoadSceneState.Payload payload = new LoadSceneState.Payload
            {
                SceneName = _staticDataModel.Config.MenuScene
            };

            _stateMachine.Enter<LoadSceneState, LoadSceneState.Payload>(payload);
        }
    }
}