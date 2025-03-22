using DebuggerOptions;
using Infrastructure.Coroutines.Runner;
using Infrastructure.Data.Models.Persistent;
using Infrastructure.Data.Models.Static;
using Infrastructure.Observers.Screen;
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
using Infrastructure.StateMachine.Game.Factory;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.UI.TransitionScreen;
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

        protected override void Configure(IContainerBuilder builder)
        {
            BindDataModels(builder);
            BindMonoServices(builder);
            BindServices(builder);
            BindScreenObserver(builder);
            BindGameStateMachine(builder);
            InitializeDebugger(builder);
            MakeInitializable(builder);
        }

        public void Initialize() => BootstrapGame();

        private void BindDataModels(IContainerBuilder builder)
        {
            builder.Register<StaticDataModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PersistentDataModel>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindMonoServices(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_coroutineRunnerPrefab, Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(_loadingScreenPrefab, Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(_transitionScreenPrefab, Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindServices(IContainerBuilder builder)
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
        }

        private void BindScreenObserver(IContainerBuilder builder) => builder.Register<ScreenObserver>(Lifetime.Singleton).AsImplementedInterfaces();

        private void BindGameStateMachine(IContainerBuilder builder)
        {
            BindGameStates(builder);
            builder.Register<GameStateFactory>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindGameStates(IContainerBuilder builder)
        {
            //chained
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<LoadDataState>(Lifetime.Singleton);
            builder.Register<SetupApplicationState>(Lifetime.Singleton);
            builder.Register<BootstrapAnalyticsState>(Lifetime.Singleton);
            builder.Register<FinalizeBootstrapState>(Lifetime.Singleton);
            builder.Register<GameLoopState>(Lifetime.Singleton);

            //other
            builder.Register<ReloadState>(Lifetime.Singleton);
            builder.Register<LoadSceneAsyncState>(Lifetime.Singleton);
            builder.Register<SaveDataState>(Lifetime.Singleton);
            builder.Register<LoadSceneWithTransitionAsyncState>(Lifetime.Singleton);
        }

        private void BootstrapGame() => Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();

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