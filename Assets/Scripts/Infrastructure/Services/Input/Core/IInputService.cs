namespace Infrastructure.Services.Input.Core
{
    public interface IInputService
    {
        public IPlayerInputActions Player { get; }
        public IUIInputActions UI { get; }

        public void SetActive(bool active);
    }
}