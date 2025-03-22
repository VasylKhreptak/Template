using DebuggerOptions;
using Infrastructure.Coroutines.Runner;
using Infrastructure.Data.Models.Persistent;
using Infrastructure.Data.Models.Static;
using Infrastructure.Services.Asset;
using Infrastructure.Services.AsyncJson;
using Infrastructure.Services.AsyncSaveLoad;
using Infrastructure.Services.AsyncScene;
using Infrastructure.Services.FixedTickable;
using Infrastructure.Services.Framerate;
using Infrastructure.Services.ID;
using Infrastructure.Services.Instantiate;
using Infrastructure.Services.Json;
using Infrastructure.Services.LateTickable;
using Infrastructure.Services.Log;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.Scene;
using Infrastructure.Services.Tickable;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.UI.TransitionScreen;
using Plugins.AudioService;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.VContainer.Scopes
{
    public class ProjectScope : LifetimeScope, IInitializable
    {
        [Header("References")]
        [SerializeField] private CoroutineRunner _coroutineRunnerPrefab;
        [SerializeField] private LoadingScreen.LoadingScreen _loadingScreenPrefab;
        [SerializeField] private TransitionScreen _transitionScreenPrefab;

        [Header("AudioService")]
        [SerializeField] private AudioService.Preferences _audioServicePreferences;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterDataModels(builder);
            RegisterMonoServices(builder);
            RegisterServices(builder);
            RegisterStateMachine(builder);
            InitializeDebugger(builder);
            MakeInitializable(builder);
        }

        public void Initialize() => Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();

        private void RegisterDataModels(IContainerBuilder builder)
        {
            builder.Register<StaticDataModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PersistentDataModel>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterMonoServices(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_coroutineRunnerPrefab, Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(_loadingScreenPrefab, Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(_transitionScreenPrefab, Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AsyncSceneService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<JsonService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AsyncJsonService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<IDService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LogService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<FramerateService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SaveLoadService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AsyncSaveLoadService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InstantiateService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AssetService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TickableService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<FixedTickableService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LateTickableService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AudioService>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(_audioServicePreferences);
        }

        private void RegisterStateMachine(IContainerBuilder builder)
        {
            RegisterStates(builder);
            builder.Register<GameStateFactory>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            //chained
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<LoadDataState>(Lifetime.Singleton);
            builder.Register<SetupApplicationState>(Lifetime.Singleton);
            builder.Register<FinalizeLoadingState>(Lifetime.Singleton);
            builder.Register<LoopState>(Lifetime.Singleton);

            //other
            builder.Register<ReloadState>(Lifetime.Singleton);
            builder.Register<LoadSceneAsyncState>(Lifetime.Singleton);
            builder.Register<SaveDataState>(Lifetime.Singleton);
            builder.Register<LoadSceneWithTransitionAsyncState>(Lifetime.Singleton);
        }

        private void InitializeDebugger(IContainerBuilder builder)
        {
            SRDebug.Init();
            builder.Register<GameOptions>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void MakeInitializable(IContainerBuilder builder)
        {
            builder.Register<IInitializable>(c => this, Lifetime.Singleton).As<IInitializable>();
        }
    }
}