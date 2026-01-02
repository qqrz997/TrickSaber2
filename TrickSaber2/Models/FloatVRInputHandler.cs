using System;
using TrickSaber2.Configuration;
using TrickSaber2.Extensions;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber2.Models;

internal class FloatVRInputHandler : IInputHandler, IVRInputHandler
{
    private InputDevice inputDevice;
    private readonly InputFeatureUsage<float> inputFeatureUsage;

    public FloatVRInputHandler(
        VRDeviceHelper.Controller controller,
        InputFeatureUsage<float> inputFeatureUsage,
        TrickConfig trickConfig)
    {
        if (controller.Node is not (XRNode.LeftHand or XRNode.RightHand))
            throw new ArgumentException($"{nameof(controller.Node)} must be either left or right hand.");
        inputDevice = controller.Device;
        this.inputFeatureUsage = inputFeatureUsage;
        Node = controller.Node;
        TrickConfig = trickConfig;
    }

    private float lastValue;
    
    public XRNode Node { get; }
    public TrickConfig TrickConfig { get; }
    public bool DeactivatedLastCheck { get; private set; }
    
    public bool CheckInputDidChange()
    {
        if (!inputDevice.TryGetFeatureValue(inputFeatureUsage, out var val))
        {
            return false;
        }
        var isActive = val > Mathf.Epsilon && val >= TrickConfig.InputConfig.ActivationThreshold;
        var didChange = !val.Approx(lastValue);
        lastValue = val;
        DeactivatedLastCheck = didChange && !isActive;
        return didChange;
    }

    public float GetInputValue() => lastValue;
}