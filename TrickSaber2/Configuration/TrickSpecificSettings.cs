using System;

namespace TrickSaber2.Configuration;

[Serializable]
internal class TrickSpecificSettings
{
    public bool SpinVelocityBased { get; set; } = true;
    public float SpinSpeedMultiplier { get; set; } = 1f;
    public float SpinPositionSmoothing { get; set; } = 0.1f;
    public float SpinRotationSmoothing { get; set; } = 0.3f;

    public float ThrowVelocityMultiplier { get; set; } = 1f;
    public float ThrowMaxReturnTime { get; set; } = 1.5f;
    public float ThrowMinReturnSpeed { get; set; } = 5f;
    public float ThrowTimeWarpMultiplier { get; set; } = 1f;
    public float ThrowTimeWarpTransitionTime { get; set; } = 1f;

    public void CopyTo(TrickSpecificSettings other)
    {
        other.SpinVelocityBased = SpinVelocityBased;
        other.SpinSpeedMultiplier = SpinSpeedMultiplier;
        other.SpinPositionSmoothing = SpinPositionSmoothing;
        other.SpinRotationSmoothing = SpinRotationSmoothing;
        other.ThrowVelocityMultiplier = ThrowVelocityMultiplier;
        other.ThrowMaxReturnTime = ThrowMaxReturnTime;
        other.ThrowMinReturnSpeed = ThrowMinReturnSpeed;
        other.ThrowTimeWarpMultiplier = ThrowTimeWarpMultiplier;
        other.ThrowTimeWarpTransitionTime = ThrowTimeWarpTransitionTime;
    }
}