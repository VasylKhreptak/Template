using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using Infrastructure.UI.Windows.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Windows.Menu
{
    public class MenuInitialWindow : BaseWindow, IWindow
    {
        [Header("Preferences")]
        [SerializeField] private GameObject _firstSelected;

        #region MonoBehaviour

        private void Awake() => Disable();

        #endregion

        public override UniTask Show()
        {
            gameObject.SetActive(true);
            RootCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(_firstSelected);
            return UniTask.CompletedTask;
        }

        public override UniTask Hide()
        {
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }

        private void Disable()
        {
            gameObject.SetActive(false);
            RootCanvasGroup.interactable = false;
        }
    }
}