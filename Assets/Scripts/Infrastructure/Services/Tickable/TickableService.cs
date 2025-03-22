using System.Collections.Generic;
using Infrastructure.Services.Tickable.Core;
using ITickable = VContainer.Unity.ITickable;

namespace Infrastructure.Services.Tickable
{
    public class TickableService : ITickableService, ITickable
    {
        private readonly HashSet<Core.ITickable> _tickables = new HashSet<Core.ITickable>();

        public void Add(Core.ITickable tickable) => _tickables.Add(tickable);

        public void Remove(Core.ITickable tickable) => _tickables.Remove(tickable);

        public void Tick()
        {
            foreach (Core.ITickable tickable in _tickables)
            {
                tickable.Tick();
            }
        }
    }
}