using System.Collections.Generic;
using Infrastructure.Services.FixedTickable.Core;
using IFixedTickable = VContainer.Unity.IFixedTickable;

namespace Infrastructure.Services.FixedTickable
{
    public class FixedTickableService : IFixedTickableService, IFixedTickable
    {
        private readonly HashSet<Core.IFixedTickable> _fixedTickables = new HashSet<Core.IFixedTickable>();

        public void Add(Core.IFixedTickable fixedTickable) => _fixedTickables.Add(fixedTickable);

        public void Remove(Core.IFixedTickable fixedTickable) => _fixedTickables.Remove(fixedTickable);

        public void FixedTick()
        {
            foreach (Core.IFixedTickable fixedTickable in _fixedTickables)
            {
                fixedTickable.FixedTick();
            }
        }
    }
}