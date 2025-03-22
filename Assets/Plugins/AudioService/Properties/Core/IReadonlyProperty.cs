namespace Plugins.AudioService.Properties.Core
{
    public interface IReadonlyProperty<in TIn, TOut>
    {
        public bool TryGet(TIn input, out TOut output);
    }
}