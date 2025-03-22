using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using VContainer;

namespace Gameplay.StateMachine.States
{
    public class GameplayStateFactory : StateFactory
    {
        public GameplayStateFactory(IObjectResolver resolver) : base(resolver) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesMap() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                //chained
                [typeof(BootstrapState)] = Resolver.Resolve<BootstrapState>,
                [typeof(LoadLevelState)] = Resolver.Resolve<LoadLevelState>,
                [typeof(FinalizeLoadingState)] = Resolver.Resolve<FinalizeLoadingState>,
                [typeof(LoopState)] = Resolver.Resolve<LoopState>

                //other
            };
    }
}