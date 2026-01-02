using System;
using BeatSaberMarkupLanguage.MenuButtons;
using Zenject;

namespace TrickSaber2.UI;

internal class MenuButtonManager : IInitializable, IDisposable
{
    private readonly MenuButtons menuButtons;
    private readonly MainFlowCoordinator mainFlowCoordinator;
    private readonly TrickSaber2FlowCoordinator flowCoordinator;
    private readonly MenuButton button;
    
    public MenuButtonManager(
        MenuButtons menuButtons,
        MainFlowCoordinator mainFlowCoordinator,
        TrickSaber2FlowCoordinator flowCoordinator)
    {
        this.menuButtons = menuButtons;
        this.mainFlowCoordinator = mainFlowCoordinator;
        this.flowCoordinator = flowCoordinator;
        button = new("Trick Saber 2", OnButtonPressed);
    }

    public void Initialize()
    {
        menuButtons.RegisterButton(button);
    }

    public void Dispose()
    {
        flowCoordinator.DidFinish -= HandleFlowCoordinatorDidFinish;
        menuButtons.UnregisterButton(button);
    }

    private void OnButtonPressed()
    {
        flowCoordinator.DidFinish += HandleFlowCoordinatorDidFinish;
        mainFlowCoordinator.PresentFlowCoordinator(flowCoordinator);
    }

    private void HandleFlowCoordinatorDidFinish()
    {
        flowCoordinator.DidFinish -= HandleFlowCoordinatorDidFinish;
        mainFlowCoordinator.DismissFlowCoordinator(flowCoordinator);
    }
}