using Gameplay.StateMachine;
using Gameplay.StateMachine.States;
using Gameplay.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using VContainer;
using VContainer.Unity;

namespace Gameplay
{
    public class GameplayScope : LifetimeScope, IInitializable
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterStateMachine(builder);
            MakeInitializable(builder);
        }

        public void Initialize() => Container.Resolve<IStateMachine<IGameplayState>>().Enter<BootstrapState>();

        private void RegisterStateMachine(IContainerBuilder builder)
        {
            RegisterStates(builder);
            builder.Register<GameplayStateFactory>(Lifetime.Singleton);
            builder.Register<GameplayStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            //chained
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<LoadLevelState>(Lifetime.Singleton);
            builder.Register<FinalizeLoadingState>(Lifetime.Singleton);
            builder.Register<LoopState>(Lifetime.Singleton);

            //other
        }

        private void MakeInitializable(IContainerBuilder builder)
        {
            builder.Register<IInitializable>(c => this, Lifetime.Singleton).As<IInitializable>();
        }
    }
}