using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace TrickSaber2.Configuration;

internal class PluginConfig
{
    public bool Enabled { get; set; } = true;

    public bool TricksAffectHitbox { get; set; } = false;
    public bool DisableDuringNotes { get; set; }
    
    [UseConverter(typeof(ListConverter<TrickConfig>))]
    public List<TrickConfig> Tricks { get; set; } = [];


    public virtual void Changed()
    {
        // Notifies BSIPA to update the config file when called
    }
}