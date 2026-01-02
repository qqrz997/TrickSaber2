using System;
using System.Linq;
using BGLib.UnityExtension;
using TrickSaber2.Components;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace TrickSaber2.Services;

internal class GameResourcesProvider : IInitializable
{
    private readonly SaberTrailRenderer trailRendererPrefab;
    private readonly GameObject saberModelPrefab;

    private GameResourcesProvider()
    {
        trailRendererPrefab = LoadAsset<SaberTrailRenderer>("Assets/Prefabs/Effects/Sabers/SaberTrailRenderer.prefab");
        saberModelPrefab = LoadPrefab("Assets/Prefabs/Sabers/BasicSaberModel.prefab");
    }

    public MockSaber CreateMockSaber()
    {
        var mockSaberObject = new GameObject("MockSaber");
        var saberObject = Object.Instantiate(saberModelPrefab, mockSaberObject.transform, false);
        saberObject.name = saberModelPrefab.name;
        saberObject.transform.localPosition = Vector3.zero;

        var saberModelController = saberObject.GetComponent<SaberModelController>();

        var saberTrail = saberObject.GetComponent<SaberTrail>();
        saberTrail._trailRendererPrefab = trailRendererPrefab;
        
        var saberBladeTopTransform = new GameObject("Bottom").transform;
        var saberBladeBottomTransform = new GameObject("Top").transform;
        saberBladeTopTransform.SetParent(saberObject.transform);
        saberBladeBottomTransform.SetParent(saberObject.transform);
        saberBladeBottomTransform.localPosition = Vector3.forward;

        return MockSaber.AddTo(mockSaberObject,
            saberModelController,
            saberTrail,
            saberBladeBottomTransform,
            saberBladeTopTransform);
    }

    public void Initialize()
    {
        trailRendererPrefab._meshRenderer = trailRendererPrefab.GetComponent<MeshRenderer>();
        trailRendererPrefab._meshFilter = trailRendererPrefab.GetComponent<MeshFilter>();
    }
    
    private static GameObject LoadPrefab(object label) =>
        AddressablesExtensions.LoadContent<GameObject>(label).FirstOrDefault() ?? throw ResourceException;
   
    private static T LoadAsset<T>(object label) where T : Object => 
        AddressablesExtensions.LoadContent<GameObject>(label).FirstOrDefault()?.GetComponent<T>() ?? throw ResourceException;
    
    private static Exception ResourceException => new InvalidOperationException("An internal resource failed to load");
}