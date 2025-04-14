using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Asset.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

public class AddressablesTest : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject _assetReference;

    private IAssetService _assetService;

    [Inject]
    public void Construct(IAssetService assetService)
    {
        _assetService = assetService;
    }

    private List<GameObject> _instances = new List<GameObject>();

    private void OnGUI()
    {
        if (GUILayout.Button("Instantiate"))
        {
            _assetService.InstantiateAsync(_assetReference).ContinueWith(x => _instances.Add(x)).Forget();
        }

        if (GUILayout.Button("Destroy"))
        {
            foreach (GameObject instance in _instances)
                Destroy(instance);

            _instances.Clear();
        }
    }
}