using TrickSaber2.Services;
using Zenject;

namespace TrickSaber2.Installers;

public class SinglePlayerInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TimeWarpManager>().AsSingle();
    }
}