using UnityEngine.XR;

namespace TrickSaber2.Models;

internal interface IVRInputHandler
{
    XRNode Node { get; }
}