using Cysharp.Threading.Tasks;

namespace Infrastructure.UI.Popups.Core
{
    public interface IPopup
    {
        public UniTask Show();

        public UniTask Hide();
    }
}