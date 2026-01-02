using UnityEngine;

namespace TrickSaber2.Components;

/// <summary>
/// Marks an object that will be used during a trick. Should only be added to objects that will be moved by tricks.
/// </summary>
internal class TrickObject : MonoBehaviour
{
    private Transform originalParent = null!;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    private void Awake()
    {
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void ResetParent()
    {
        transform.SetParent(originalParent);
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}