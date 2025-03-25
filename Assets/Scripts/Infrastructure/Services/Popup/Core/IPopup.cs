using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Popup.Core
{
    public interface IPopup
    {
        public UniTask Show();

        public UniTask Hide();
    }
}