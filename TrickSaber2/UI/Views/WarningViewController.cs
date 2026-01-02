using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using TMPro;

namespace TrickSaber2.UI.Views;

[HotReload(RelativePathToLayout = @".\Warning.bsml")]
[ViewDefinition("TrickSaber2.UI.Views.Warning.bsml")]
internal class WarningViewController : BSMLAutomaticViewController
{
    [UIComponent("WarningMessageText")] private readonly TextMeshProUGUI warningMessageText = null!;

    private ViewController? lastViewController;
    
    public event Action<ViewController>? DidFinish;
    
    public void SetLastViewController(ViewController viewController)
    {
        lastViewController = viewController;
    }
    
    public void ShowWarning(string message)
    {
        warningMessageText.text = message;
    }

    public void OkButtonPressed()
    {
        if (lastViewController != null)
        {
            DidFinish?.Invoke(lastViewController);
        }
        else
        {
            warningMessageText.text = "<color=red>Previous ViewController is not set...";
        }
    }
}