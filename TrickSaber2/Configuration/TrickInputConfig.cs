using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

namespace TrickSaber2.Configuration;

internal class TrickInputConfig
{
    [UseConverter(typeof(EnumConverter<InputType>))]
    public InputType Type { get; set; }

    // How much the input has to be pressed (0-1) for the trick to activate
    public float ActivationThreshold { get; set; } = 0.3f;
    
    // Should the trick be activated upon releasing the input
    public bool ActivateOnRelease { get; set; }
    
    // Whether the trick should only activate for the duration of the input
    public bool Toggle { get; set; }
    
    // How long to wait before starting the trick after an input
    public float ActivationDelay { get; set; }
    
    // How long to wait before stopping the trick
    public float DeactivationDelay { get; set; }

    public void CopyTo(TrickInputConfig other)
    {
        other.Type = Type;
        other.ActivationThreshold = ActivationThreshold;
        other.ActivateOnRelease = ActivateOnRelease;
        other.Toggle = Toggle;
        other.ActivationDelay = ActivationDelay;
        other.DeactivationDelay = DeactivationDelay;
    }
}