using UnityEngine.InputSystem;

namespace Infrastructure.Services.Input.Core
{
    public interface IInputService
    {
        public InputActions Actions { get; }

        public void SetActive(bool active);
    }
}