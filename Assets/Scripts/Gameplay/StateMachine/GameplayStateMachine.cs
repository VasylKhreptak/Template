using Gameplay.StateMachine.States;
using Gameplay.StateMachine.States.Core;
using Infrastructure.StateMachine.Main;

namespace Gameplay.StateMachine
{
    public class GameplayStateMachine : StateMachine<IGameplayState>
    {
        protected GameplayStateMachine(GameplayStateFactory stateFactory) : base(stateFactory) { }
    }
}