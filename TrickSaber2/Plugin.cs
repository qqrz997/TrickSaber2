using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using SiraUtil.Zenject;
using TrickSaber2.Configuration;
using TrickSaber2.Installers;
using Logger = IPA.Logging.Logger;

namespace TrickSaber2;

[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
internal class Plugin
{
    public static Logger Log { get; private set; } = null!;

    [Init]
    public Plugin(Logger ipaLogger, Config config, PluginMetadata pluginMetadata, Zenjector zenjector)
    {
        Log = ipaLogger;
        
        zenjector.Install<AppInstaller>(Location.App, config.Generated<PluginConfig>());
        zenjector.Install<MenuInstaller>(Location.Menu);
        zenjector.Install<PlayerInstaller>(Location.Player);
        zenjector.Install<SinglePlayerInstaller>(Location.Singleplayer);
        
        Log.Info($"{pluginMetadata.Name} {pluginMetadata.HVersion} initialized.");
    }
}