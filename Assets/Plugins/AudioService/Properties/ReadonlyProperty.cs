using System;
using Plugins.AudioService.Properties.Core;

namespace Plugins.AudioService.Properties
{
    public class ReadonlyProperty<TIn, TValue> : IReadonlyProperty<TIn, TValue>
    {
        private readonly Func<TIn, bool> _canAccess;
        private readonly Func<TIn, TValue> _get;

        public ReadonlyProperty(Func<TIn, bool> canAccess, Func<TIn, TValue> get)
        {
            _get = get;
            _canAccess = canAccess;
        }

        public bool TryGet(TIn input, out TValue output)
        {
            if (_canAccess.Invoke(input))
            {
                output = _get.Invoke(input);
                return true;
            }

            output = default;
            return false;
        }
    }
}