using System;
using Plugins.AudioService.Properties.Core;

namespace Plugins.AudioService.Properties
{
    public class Property<TIn, TValue> : ReadonlyProperty<TIn, TValue>, IProperty<TIn, TValue>
    {
        private readonly Func<TIn, bool> _canAccess;
        private readonly Action<TIn, TValue> _set;

        public Property(Func<TIn, bool> canAccess, Func<TIn, TValue> get, Action<TIn, TValue> set) : base(canAccess, get)
        {
            _set = set;
            _canAccess = canAccess;
        }

        public bool TrySet(TIn input, TValue value)
        {
            if (_canAccess.Invoke(input))
            {
                _set.Invoke(input, value);
                return true;
            }

            return false;
        }
    }
}