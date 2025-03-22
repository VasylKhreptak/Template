using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using VContainer;

namespace Infrastructure.StateMachine.Game.Factory
{
    public class GameStateFactory : StateFactory
    {
        public GameStateFactory(IObjectResolver resolver) : base(resolver) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesMap() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                //chained
                [typeof(BootstrapState)] = Resolver.Resolve<BootstrapState>,
                [typeof(LoadDataState)] = Resolver.Resolve<LoadDataState>,
                [typeof(SetupApplicationState)] = Resolver.Resolve<SetupApplicationState>,
                [typeof(BootstrapAnalyticsState)] = Resolver.Resolve<BootstrapAnalyticsState>,
                [typeof(FinalizeBootstrapState)] = Resolver.Resolve<FinalizeBootstrapState>,
                [typeof(GameLoopState)] = Resolver.Resolve<GameLoopState>,

                //other
                [typeof(ReloadState)] = Resolver.Resolve<ReloadState>,
                [typeof(SaveDataState)] = Resolver.Resolve<SaveDataState>,
                [typeof(LoadSceneAsyncState)] = Resolver.Resolve<LoadSceneAsyncState>,
                [typeof(LoadSceneWithTransitionAsyncState)] = Resolver.Resolve<LoadSceneWithTransitionAsyncState>
            };
    }
}