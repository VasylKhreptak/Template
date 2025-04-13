using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressablesTest : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject _assetReference;

    // private IAssetService _assetService;
    //
    // [Inject]
    // public void Construct(IAssetService assetService)
    // {
    //     _assetService = assetService;
    // }

    private readonly List<GameObject> _instances = new List<GameObject>();

    private void OnGUI()
    {
        if (GUILayout.Button("Instantiate"))
        {
            // if (_assetService == null)
            Addressables.InstantiateAsync(_assetReference).ToUniTask().ContinueWith(x => _instances.Add(x)).Forget();
            // else
            // _assetService.InstantiateAsync(_assetReference).ContinueWith(x => _instances.Add(x)).Forget();
        }

        if (GUILayout.Button("Destroy"))
        {
            // if (_assetService == null)
            // {

            foreach (GameObject instance in _instances)
                Addressables.ReleaseInstance(instance);

            // }
            // else
            // {
            // foreach (GameObject instance in _instances)
            // Destroy(instance);
            // }

            _instances.Clear();
        }
    }
}