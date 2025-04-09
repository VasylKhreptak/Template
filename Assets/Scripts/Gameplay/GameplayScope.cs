using Gameplay.StateMachine;
using Gameplay.StateMachine.States;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Window;
using Infrastructure.Services.Window.Core;
using Infrastructure.Services.Window.Factories;
using Infrastructure.StateMachine.Main.Core;
using VContainer;
using VContainer.Unity;

namespace Gameplay
{
    public class GameplayScope : LifetimeScope, IInitializable
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterStateMachine(builder);
            MakeInitializable(builder);
        }

        public void Initialize() => Container.Resolve<IStateMachine<IGameplayState>>().Enter<BootstrapState>();

        private void RegisterServices(IContainerBuilder builder)
        {
            RegisterWindowService(builder);
        }

        private void RegisterWindowService(IContainerBuilder builder)
        {
            builder.Register<WindowFactory>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<WindowService>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void RegisterStateMachine(IContainerBuilder builder)
        {
            RegisterStates(builder);
            builder.Register<GameplayStateFactory>(Lifetime.Singleton);
            builder.Register<GameplayStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            IWindowService parentWindowService = Parent.Container.Resolve<IWindowService>();

            //chained
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<LoadLevelState>(Lifetime.Singleton);
            builder.Register<SetupUIState>(Lifetime.Singleton);
            builder.Register<FinalizeLoadingState>(Lifetime.Singleton).WithParameter(parentWindowService);
            builder.Register<LoopState>(Lifetime.Singleton);

            //other
            builder.Register<SaveDataState>(Lifetime.Singleton);
            builder.Register<LoadMenuState>(Lifetime.Singleton).WithParameter(parentWindowService);
        }

        private void MakeInitializable(IContainerBuilder builder)
        {
            builder.Register<IInitializable>(c => this, Lifetime.Singleton).As<IInitializable>();
        }
    }
}