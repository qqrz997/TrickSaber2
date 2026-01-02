using System.Collections.Generic;
using UnityEngine.XR;

namespace TrickSaber2.Extensions;

internal class VRDeviceHelper
{
    // todo: handle connection and disconnection of controllers
    
    public static IEnumerable<Controller> GetControllers()
    {
        var leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (leftHandDevice.isValid) yield return new(leftHandDevice, XRNode.LeftHand);
        var rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightHandDevice.isValid) yield return new(rightHandDevice, XRNode.RightHand);
    }
    
    public record Controller(InputDevice Device, XRNode Node);
}