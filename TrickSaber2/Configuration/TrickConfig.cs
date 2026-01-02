using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using JetBrains.Annotations;

namespace TrickSaber2.Configuration;

internal class TrickConfig
{
    [UseConverter(typeof(EnumConverter<TrickType>))]
    public TrickType Type { get; set; }

    public TrickSpecificSettings TrickSpecificSettings { get; set; } = new();
    
    public TrickInputConfig InputConfig { get; set; } = new();

    public TrickConfig(TrickType trickType, InputType inputType)
    {
        Type = trickType;
        InputConfig = new() { Type = inputType };
    }

    [UsedImplicitly]
    public TrickConfig() { }

    public void CopyTo(TrickConfig other)
    {
        other.Type = Type;
        TrickSpecificSettings.CopyTo(other.TrickSpecificSettings);
        InputConfig.CopyTo(other.InputConfig);
    }
}