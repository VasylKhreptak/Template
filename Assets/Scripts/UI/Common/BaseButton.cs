using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public abstract class BaseButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        #region MonoBehaviour

        private void OnValidate() => _button ??= GetComponent<Button>();

        private void OnEnable() => _button.onClick.AddListener(OnClick);

        private void OnDisable() => _button.onClick.RemoveListener(OnClick);

        #endregion

        protected abstract void OnClick();
    }
}