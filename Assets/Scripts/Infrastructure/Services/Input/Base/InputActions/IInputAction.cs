namespace Infrastructure.Services.Input.Base.InputActions
{
    public interface IInputAction<out T>
    {
        public bool Enabled { get; set; }

        public T Value { get; }
    }
}