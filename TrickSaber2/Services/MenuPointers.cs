using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrickSaber2.Services;

internal class MenuPointers
{
    private readonly GameObject leftPointer;
    private readonly GameObject rightPointer;
    private readonly List<MeshRenderer> meshRenderers = [];

    private MenuPointers()
    {
        var controllers = Resources.FindObjectsOfTypeAll<VRController>();
        leftPointer = controllers.First(c => c.transform.name == "ControllerLeft").transform.Find("MenuHandle").gameObject;
        rightPointer = controllers.First(c => c.transform.name == "ControllerRight").transform.Find("MenuHandle").gameObject;

        meshRenderers.AddRange(GetMenuHandleRenderers(leftPointer));
        meshRenderers.AddRange(GetMenuHandleRenderers(rightPointer));
    }

    public Transform LeftParent => leftPointer.transform;
    public Transform RightParent => rightPointer.transform;
    
    public void SetPointerVisibility(bool visible)
    {
        for (var i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].enabled = visible;
        }
    }

    private static IEnumerable<MeshRenderer> GetMenuHandleRenderers(GameObject menuHandle)
    {
        yield return menuHandle.transform.Find("Glowing").GetComponent<MeshRenderer>();
        yield return menuHandle.transform.Find("Normal").GetComponent<MeshRenderer>();
        yield return menuHandle.transform.Find("FakeGlow0").GetComponent<MeshRenderer>();
        yield return menuHandle.transform.Find("FakeGlow1").GetComponent<MeshRenderer>();
    }
}