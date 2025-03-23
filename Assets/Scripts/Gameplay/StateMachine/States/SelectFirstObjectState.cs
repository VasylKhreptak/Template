using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.StateMachine.States
{
    public class SelectFirstObjectState : IGameplayState, IState
    {
        private readonly IStateMachine<IGameplayState> _stateMachine;
        private readonly ILogService _logService;
        private readonly GameObject _objectToSelect;

        public SelectFirstObjectState(IStateMachine<IGameplayState> stateMachine, ILogService logService, GameObject objectToSelect)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _objectToSelect = objectToSelect;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.SelectFirstObjectState.Enter");
            EventSystem.current.SetSelectedGameObject(_objectToSelect);
            _stateMachine.Enter<FinalizeLoadingState>();
        }
    }
}