using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Popup.Core
{
    public interface IPopupService
    {
        public UniTask<IPopup> Show(PopupID id);

        public void DestroyAll();
    }
}