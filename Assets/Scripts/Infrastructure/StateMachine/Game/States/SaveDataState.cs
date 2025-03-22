using System;
using Infrastructure.Data.Models.Persistent.Core;
using Infrastructure.Services.AsyncSaveLoad.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class SaveDataState : IGameState, IPayloadedState<Action>
    {
        private const string Key = "Data";

        private readonly IPersistentDataModel _persistentDataModel;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ILogService _logService;
        private readonly IAsyncSaveLoadService _saveLoadService;

        public SaveDataState(IPersistentDataModel persistentDataModel, IStateMachine<IGameState> gameStateMachine, ILogService logService,
            IAsyncSaveLoadService saveLoadService)
        {
            _persistentDataModel = persistentDataModel;
            _gameStateMachine = gameStateMachine;
            _logService = logService;
            _saveLoadService = saveLoadService;
        }

        public async void Enter(Action onComplete)
        {
            _logService.Log("SaveDataState");

            await _saveLoadService.SaveAsync(Key, _persistentDataModel.Data);

            _logService.Log("Saved local data");

            _gameStateMachine.Enter<GameLoopState>();
            onComplete?.Invoke();
        }
    }
}