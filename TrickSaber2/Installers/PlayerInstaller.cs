using TrickSaber2.Components;
using TrickSaber2.Configuration;
using TrickSaber2.Services;
using Zenject;

namespace TrickSaber2.Installers;

internal class PlayerInstaller : Installer
{
    private readonly PluginConfig pluginConfig;

    public PlayerInstaller(PluginConfig pluginConfig)
    {
        this.pluginConfig = pluginConfig;
    }

    public override void InstallBindings()
    {
        if (!pluginConfig.Enabled)
        {
            return;
        }

        Container.BindInterfacesTo<ScoreSubmissionsManager>().AsSingle();
        Container.Bind<GameTrickManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        Container.Bind<GameTrickObjectManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<NoteTracker>().AsSingle();
    }
}