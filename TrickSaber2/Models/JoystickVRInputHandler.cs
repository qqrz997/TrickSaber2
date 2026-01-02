using System;
using TrickSaber2.Configuration;
using TrickSaber2.Extensions;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber2.Models;

internal class JoystickVRInputHandler : IInputHandler, IVRInputHandler
{
    private InputDevice inputDevice;
    private readonly InputFeatureUsage<Vector2> inputFeatureUsage;
    private readonly Axis axis;

    public JoystickVRInputHandler(
        VRDeviceHelper.Controller controller,
        InputFeatureUsage<Vector2> inputFeatureUsage,
        TrickConfig trickConfig,
        Axis axis)
    {
        if (controller.Node is not (XRNode.LeftHand or XRNode.RightHand))
            throw new ArgumentException($"{nameof(controller.Node)} must be either left or right hand.");
        inputDevice = controller.Device;
        this.inputFeatureUsage = inputFeatureUsage;
        this.axis = axis;
        Node = controller.Node;
        TrickConfig = trickConfig;
    }
    
    private float lastValue;
    
    public XRNode Node { get; }
    public TrickConfig TrickConfig { get; }
    public bool DeactivatedLastCheck { get; private set; }
    
    public bool CheckInputDidChange()
    {
        if (!inputDevice.TryGetFeatureValue(inputFeatureUsage, out var vector))
        {
            return false;
        }
        var val = axis switch
        {
            Axis.X => vector.x,
            Axis.Y => vector.y,
            _ => throw new ArgumentOutOfRangeException()
        };
        var isActive = val > Mathf.Epsilon && val >= TrickConfig.InputConfig.ActivationThreshold;
        var didChange = !Mathf.Approximately(val, lastValue);                       
        lastValue = val;
        DeactivatedLastCheck = didChange && !isActive;
        return didChange;
    }

    public float GetInputValue() => lastValue;
}