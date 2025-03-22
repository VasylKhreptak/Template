using Infrastructure.StateMachine.Main.Core;
using Menu.StateMachine;
using Menu.StateMachine.States;
using Menu.StateMachine.States.Core;
using VContainer;
using VContainer.Unity;

namespace Menu
{
    public class MenuScope : LifetimeScope, IInitializable
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterStateMachine(builder);
            MakeInitializable(builder);
        }

        public void Initialize() => Container.Resolve<IStateMachine<IMenuState>>().Enter<BootstrapState>();

        private void RegisterStateMachine(IContainerBuilder builder)
        {
            RegisterStates(builder);
            builder.Register<MenuStateFactory>(Lifetime.Singleton);
            builder.Register<MenuStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<FinalizeLoadingState>(Lifetime.Singleton);
            builder.Register<LoopState>(Lifetime.Singleton);
        }

        private void MakeInitializable(IContainerBuilder builder)
        {
            builder.Register<IInitializable>(c => this, Lifetime.Singleton).As<IInitializable>();
        }
    }
}