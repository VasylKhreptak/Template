using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Popup.Core
{
    public interface IConfirmationPopup : IPopup
    {
        public UniTask<ConfirmationPopupResult> ResultTask { get; }
    }
}