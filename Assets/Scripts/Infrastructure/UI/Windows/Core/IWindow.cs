using Cysharp.Threading.Tasks;

namespace Infrastructure.UI.Windows.Core
{
    public interface IWindow
    {
        public UniTask Show();

        public UniTask Hide();
    }
}