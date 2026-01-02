using System;
using TrickSaber2.Configuration;
using TrickSaber2.Extensions;
using UnityEngine.XR;

namespace TrickSaber2.Models;

internal class BoolVRInputHandler : IInputHandler, IVRInputHandler
{
    private InputDevice inputDevice;
    private readonly InputFeatureUsage<bool> inputFeatureUsage;

    public BoolVRInputHandler(
        VRDeviceHelper.Controller controller,
        InputFeatureUsage<bool> inputFeatureUsage,
        TrickConfig trickConfig)
    {
        if (controller.Node is not (XRNode.LeftHand or XRNode.RightHand))
            throw new ArgumentException($"{nameof(controller.Node)} must be either left or right hand.");
        inputDevice = controller.Device;
        this.inputFeatureUsage = inputFeatureUsage;
        Node = controller.Node;
        TrickConfig = trickConfig;
    }
    
    private bool lastValue;
    
    public XRNode Node { get; }
    public TrickConfig TrickConfig { get; }
    public bool DeactivatedLastCheck { get; private set; }
    
    public bool CheckInputDidChange()
    {
        if (!inputDevice.TryGetFeatureValue(inputFeatureUsage, out var isActive))
        {
            return false;
        }
        var didChange = lastValue != isActive;
        lastValue = isActive;
        DeactivatedLastCheck = didChange && !isActive;
        return didChange;
    }

    public float GetInputValue() => lastValue ? 1f : 0f;
}