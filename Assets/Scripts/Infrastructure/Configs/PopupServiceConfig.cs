using System.Collections.Generic;
using Infrastructure.Services.Popup.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Configs
{
    [CreateAssetMenu(fileName = "PopupServiceConfig", menuName = "Configs/PopupServiceConfig")]
    public class PopupServiceConfig : SerializedScriptableObject
    {
        [Header("References")]
        [SerializeField] private Dictionary<PopupID, AssetReferenceGameObject> _popupsMap = new Dictionary<PopupID, AssetReferenceGameObject>();
        [SerializeField] private GameObject _containerPrefab;
        [SerializeField] private GameObject _inputBlockerPrefab;

        public GameObject ContainerPrefab => _containerPrefab;
        public GameObject InputBlockerPrefab => _inputBlockerPrefab;

        public AssetReferenceGameObject GetAssetReference(PopupID id)
        {
            if (_popupsMap.TryGetValue(id, out AssetReferenceGameObject prefab))
                return prefab;

            throw new KeyNotFoundException("Popup Type: " + id);
        }
    }
}