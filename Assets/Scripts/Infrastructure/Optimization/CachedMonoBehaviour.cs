using Infrastructure.Services.FixedTickable.Core;
using Infrastructure.Services.LateTickable.Core;
using Infrastructure.Services.Tickable.Core;
using UnityEngine;
using VContainer;
using IFixedTickable = Infrastructure.Services.FixedTickable.Core.IFixedTickable;
using ILateTickable = Infrastructure.Services.LateTickable.Core.ILateTickable;
using ITickable = Infrastructure.Services.Tickable.Core.ITickable;

namespace Infrastructure.Optimization
{
    public class CachedMonoBehaviour : MonoBehaviour
    {
        private ITickableService _tickableService;
        private IFixedTickableService _fixedTickableService;
        private ILateTickableService _lateTickableService;

        [Inject]
        public void Construct(ITickableService tickableService, IFixedTickableService fixedTickableService, ILateTickableService lateTickableService)
        {
            _tickableService = tickableService;
            _fixedTickableService = fixedTickableService;
            _lateTickableService = lateTickableService;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            if (this is ITickable tickable)
                _tickableService.Add(tickable);

            if (this is IFixedTickable fixedTickable)
                _fixedTickableService.Add(fixedTickable);

            if (this is ILateTickable lateTickable)
                _lateTickableService.Add(lateTickable);
        }

        private void OnDisable()
        {
            if (this is ITickable tickable)
                _tickableService.Remove(tickable);

            if (this is IFixedTickable fixedTickable)
                _fixedTickableService.Remove(fixedTickable);

            if (this is ILateTickable lateTickable)
                _lateTickableService.Remove(lateTickable);
        }

        #endregion
    }
}