using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Menu.StateMachine.States.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu.StateMachine.States
{
    public class SelectFirstObjectState : IMenuState, IState
    {
        private readonly IStateMachine<IMenuState> _stateMachine;
        private readonly ILogService _logService;
        private readonly GameObject _objectToSelect;

        public SelectFirstObjectState(IStateMachine<IMenuState> stateMachine, ILogService logService, GameObject objectToSelect)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _objectToSelect = objectToSelect;
        }

        public void Enter()
        {
            _logService.Log("Menu.SelectFirstObjectState.Enter");
            EventSystem.current.SetSelectedGameObject(_objectToSelect);
            _stateMachine.Enter<FinalizeLoadingState>();
        }
    }
}