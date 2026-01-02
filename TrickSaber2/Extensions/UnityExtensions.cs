using UnityEngine;

namespace TrickSaber2.Extensions;

internal static class UnityExtensions
{
    public static TComponent GetOrAddComponent<TComponent>(this GameObject go) where TComponent : Component
    {
        var component = go.GetComponent<TComponent>();
        return component ? component : go.AddComponent<TComponent>();
    }

    public static Vector3 GetClosestAxis(in this Vector3 v)
    {
        var x = Mathf.Abs(v.x);
        var y = Mathf.Abs(v.y);
        var z = Mathf.Abs(v.z);

        if (x >= y && x >= z)
        {
            return v.x >= 0 ? Vector3.right : Vector3.left;
        }

        if (y >= z && y >= x)
        {
            return v.y >= 0 ? Vector3.up : Vector3.down;
        }
        
        return v.z >= 0 ? Vector3.forward : Vector3.back;
    }

    public static bool Approx(this float a, float b) => Mathf.Approximately(a, b);
}