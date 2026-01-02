using System.Collections;
using TrickSaber2.Extensions;
using UnityEngine;

namespace TrickSaber2.Components;

internal class MockSaber : MonoBehaviour
{
    [SerializeField]
    private SaberModelController saberModelController = null!;

    [SerializeField]
    private SaberTrail saberTrail = null!;

    [SerializeField]
    private Transform saberBladeTopTransform = null!;
    
    [SerializeField]
    private Transform saberBladeBottomTransform = null!;
    
    private readonly SaberMovementData saberMovementData = new();
    
    public static MockSaber AddTo(GameObject go,
        SaberModelController saberModelController,
        SaberTrail saberTrail,
        Transform saberBladeTopTransform,
        Transform saberBladeBottomTransform)
    {
        var mockSaber = go.AddComponent<MockSaber>();
        mockSaber.saberModelController = saberModelController;
        mockSaber.saberTrail = saberTrail;
        mockSaber.saberTrail._movementData = mockSaber.saberMovementData;
        mockSaber.saberBladeTopTransform = saberBladeTopTransform;
        mockSaber.saberBladeBottomTransform = saberBladeBottomTransform;
        return mockSaber;
    }

    public void SetColor(Color color)
    {
        StartCoroutine(SetColorNextFrame(color));
        saberTrail._color = color;
    }

    private IEnumerator SetColorNextFrame(Color color)
    {
        yield return null;
        saberModelController.SetNewColor(color);
    }

    private void Update()
    {
        saberMovementData.AddNewData(
            saberBladeTopTransform.position,
            saberBladeBottomTransform.position,
            Time.time);
    }
}