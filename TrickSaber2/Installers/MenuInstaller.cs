using BeatSaberMarkupLanguage.Tags;
using TrickSaber2.Services;
using TrickSaber2.UI;
using TrickSaber2.UI.Components;
using TrickSaber2.UI.Views;
using Zenject;

namespace TrickSaber2.Installers;

internal class MenuInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<MenuButtonManager>().AsSingle();
        Container.Bind<TrickSaber2FlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
        
        Container.Bind<MainViewController>().FromNewComponentAsViewController().AsSingle();
        Container.Bind<BindingCreationViewController>().FromNewComponentAsViewController().AsSingle();
        Container.Bind<BindingDeleteViewController>().FromNewComponentAsViewController().AsSingle();
        Container.Bind<WarningViewController>().FromNewComponentAsViewController().AsSingle();
        Container.Bind<TrickEditorViewController>().FromNewComponentAsViewController().AsSingle();
        
        Container.Bind<MenuPointers>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuSabersManager>().AsSingle();
        
        // Components
        Container.Bind<BSMLTag>().To<BetterScrollViewTag>().AsSingle();
    }
}