namespace Plugins.AudioService.Properties.Core
{
    public interface IProperty<in TIn, TValue> : IReadonlyProperty<TIn, TValue>
    {
        public bool TrySet(TIn input, TValue value);
    }
}