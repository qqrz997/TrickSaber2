using TrickSaber2.Configuration;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber2.Models;

internal class Vector2InputHandler
{
    private InputDevice inputDevice;
    private readonly InputFeatureUsage<Vector2> inputFeatureUsage;
    private readonly TrickInputConfig config;

    public Vector2InputHandler(
        InputDevice inputDevice,
        InputFeatureUsage<Vector2> inputFeatureUsage,
        TrickInputConfig config)
    {
        this.inputDevice = inputDevice;
        this.inputFeatureUsage = inputFeatureUsage;
        this.config = config;
    }
}