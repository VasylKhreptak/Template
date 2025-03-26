using Infrastructure.Services.Window.Core;
using UI.Common;
using VContainer;

namespace Infrastructure.UI.Buttons
{
    public class CloseWindowButton : BaseButton
    {
        private IWindowService _windowService;

        [Inject]
        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }

        protected override void OnClick() => _windowService.TopWindow.Value?.Hide();
    }
}