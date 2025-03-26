using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Infrastructure.Services.Window
{
    public class InputBlocker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        public RectTransform RectTransform => _rectTransform;

        #region MonoBehaviour

        private void OnValidate() => _rectTransform ??= GetComponent<RectTransform>();

        private void OnDestroy() => _subscriptions.Clear();

        #endregion

        public void LinkTo(GameObject target)
        {
            _subscriptions.Clear();

            target.OnEnableAsObservable().Subscribe(_ => gameObject.SetActive(true)).AddTo(_subscriptions);
            target.OnDisableAsObservable().Subscribe(_ => gameObject.SetActive(false)).AddTo(_subscriptions);
            target.OnDestroyAsObservable().Subscribe(_ => Destroy(gameObject)).AddTo(_subscriptions);
        }
    }
}