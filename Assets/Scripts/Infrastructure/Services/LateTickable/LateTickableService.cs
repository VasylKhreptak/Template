using System.Collections.Generic;
using Infrastructure.Services.LateTickable.Core;
using ILateTickable = VContainer.Unity.ILateTickable;

namespace Infrastructure.Services.LateTickable
{
    public class LateTickableService : ILateTickableService, ILateTickable
    {
        private readonly HashSet<Core.ILateTickable> _lateTickables = new HashSet<Core.ILateTickable>();

        public void Add(Core.ILateTickable lateTickable) => _lateTickables.Add(lateTickable);

        public void Remove(Core.ILateTickable lateTickable) => _lateTickables.Remove(lateTickable);

        public void LateTick()
        {
            foreach (Core.ILateTickable lateTickable in _lateTickables)
            {
                lateTickable.LateTick();
            }
        }
    }
}