using System;
using HMUI;
using TrickSaber2.Configuration;
using TrickSaber2.Services;
using TrickSaber2.UI.Views;
using Zenject;
using static HMUI.ViewController;

namespace TrickSaber2.UI;

internal class TrickSaber2FlowCoordinator : FlowCoordinator
{
    [Inject] private readonly PluginConfig pluginConfig = null!;
    [Inject] private readonly MenuSabersManager menuSabersManager = null!;
    [Inject] private readonly MainViewController mainViewController = null!;
    [Inject] private readonly BindingCreationViewController bindingCreationViewController = null!;
    [Inject] private readonly BindingDeleteViewController bindingDeleteViewController = null!;
    [Inject] private readonly WarningViewController warningViewController = null!;
    [Inject] private readonly TrickEditorViewController trickEditorViewController = null!;
    
    public event Action? DidFinish;

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        if (addedToHierarchy)
        {
            mainViewController.DidFinish += HandleMainViewControllerDidFinish;
            bindingCreationViewController.DidFinish += HandleBindingCreationViewControllerDidFinish;
            bindingDeleteViewController.DidFinish += HandleBindingDeleteViewControllerDidFinish;
            warningViewController.DidFinish += HandleWarningViewControllerDidFinish;
            trickEditorViewController.DidFinish += HandleTrickEditorViewControllerDidFinish;
            menuSabersManager.SwitchToSabers();
        }
        showBackButton = true;
        SetTitle("TrickSaber 2");
        ProvideInitialViewControllers(mainViewController);
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
        if (removedFromHierarchy)
        {
            mainViewController.DidFinish -= HandleMainViewControllerDidFinish;
            bindingCreationViewController.DidFinish -= HandleBindingCreationViewControllerDidFinish;
            bindingDeleteViewController.DidFinish -= HandleBindingDeleteViewControllerDidFinish;
            warningViewController.DidFinish -= HandleWarningViewControllerDidFinish;
            trickEditorViewController.DidFinish -= HandleTrickEditorViewControllerDidFinish;
            pluginConfig.Changed();
            menuSabersManager.SwitchToPointers();
        }
    }

    protected override void TopViewControllerWillChange(
        ViewController oldViewController, ViewController newViewController, AnimationType animationType)
    {
        showBackButton = newViewController is MainViewController;
    }

    protected override void BackButtonWasPressed(ViewController topViewController)
    {
        DidFinish?.Invoke();
    }

    private void HandleMainViewControllerDidFinish(MainViewController.FinishResult result)
    {
        if (result is MainViewController.Add)
        {
            ReplaceTopViewController(bindingCreationViewController, animationDirection: AnimationDirection.Vertical);
        }
        else if (result is MainViewController.Edit edit)
        {
            trickEditorViewController.SetConfigForEditing(edit.Config);
            ReplaceTopViewController(trickEditorViewController, animationDirection: AnimationDirection.Vertical);
        }
        else if (result is MainViewController.Delete delete)
        {
            ReplaceTopViewController(bindingDeleteViewController, animationDirection: AnimationDirection.Vertical);
            bindingDeleteViewController.SetConfigForDeletion(delete.Config);
        }
    }

    private void HandleBindingCreationViewControllerDidFinish(BindingCreationViewController.FinishResult result)
    {
        if (result is BindingCreationViewController.Warning warning)
        {
            ReplaceTopViewController(warningViewController, animationDirection: AnimationDirection.Vertical);
            warningViewController.SetLastViewController(bindingCreationViewController);
            warningViewController.ShowWarning(warning.Message);
        }
        else
        {
            if (result is BindingCreationViewController.Success)
            {
                menuSabersManager.UpdateInputListeners();
            }
            ReplaceTopViewController(mainViewController, animationDirection: AnimationDirection.Vertical);
        }
    }
    
    private void HandleBindingDeleteViewControllerDidFinish(BindingDeleteViewController.FinishResult result)
    {
        if (result is BindingDeleteViewController.Confirm confirm 
            && pluginConfig.Tricks.Remove(confirm.ConfigForDeletion))
        {
            mainViewController.RefreshList();
        }
        ReplaceTopViewController(mainViewController, animationDirection: AnimationDirection.Vertical);
    }

    private void HandleWarningViewControllerDidFinish(ViewController viewController)
    {
        ReplaceTopViewController(viewController, animationDirection: AnimationDirection.Vertical);
    }

    private void HandleTrickEditorViewControllerDidFinish(TrickEditorViewController.FinishResult result)
    {
        if (result is TrickEditorViewController.Warning warning)
        {
            ReplaceTopViewController(warningViewController, animationDirection: AnimationDirection.Vertical);
            warningViewController.SetLastViewController(trickEditorViewController);
            warningViewController.ShowWarning(warning.Message);
        }
        else
        {
            ReplaceTopViewController(mainViewController, animationDirection: AnimationDirection.Vertical);
        }
    }
}