﻿using Infrastructure.Services.FixedTickable.Core;
using Infrastructure.Services.LateTickable.Core;
using Infrastructure.Services.Tickable.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Infrastructure.Optimization
{
    public abstract class CachedSerializedMonoBehaviour : SerializedMonoBehaviour
    {
        private ITickableService _tickableService;
        private IFixedTickableService _fixedTickableService;
        private ILateTickableService _lateTickableService;

        protected void Construct(ITickableService tickableService, IFixedTickableService fixedTickableService, ILateTickableService lateTickableService)
        {
            _tickableService = tickableService;
            _fixedTickableService = fixedTickableService;
            _lateTickableService = lateTickableService;

            _constructed = true;
        }

        private bool _constructed;

        #region MonoBehaviour

        private void Awake()
        {
            if (_constructed == false)
                Debug.LogError("CachedMonoBehaviour was not constructed!", gameObject);
        }

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