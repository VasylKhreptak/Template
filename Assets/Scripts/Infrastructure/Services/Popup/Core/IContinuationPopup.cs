using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Popup.Core
{
    public interface IContinuationPopup : IPopup
    {
        public UniTask ContinueTask { get; }
    }
}