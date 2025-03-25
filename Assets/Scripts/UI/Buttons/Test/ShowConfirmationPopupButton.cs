using Cysharp.Threading.Tasks;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.Popup.Core;
using UI.Common;
using VContainer;

namespace UI.Buttons.Test
{
    public class ShowConfirmationPopupButton : BaseButton
    {
        private IPopupService _popupService;
        private ILogService _logService;

        [Inject]
        public void Construct(IPopupService popupService, ILogService logService)
        {
            _popupService = popupService;
            _logService = logService;
        }

        protected override void OnClick()
        {
            _popupService
                .Show(PopupID.TestConfirmation)
                .ContinueWith(popup =>
                {
                    IConfirmationPopup continuationPopup = (IConfirmationPopup)popup;
                    continuationPopup.ResultTask.ContinueWith(result => _logService.Log("Popup result: " + result)).Forget();
                })
                .Forget();
        }
    }
}