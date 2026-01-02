using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using TrickSaber2.Configuration;

namespace TrickSaber2.UI.Views;

[HotReload(RelativePathToLayout = @".\BindingDelete.bsml")]
[ViewDefinition("TrickSaber2.UI.Views.BindingDelete.bsml")]
internal class BindingDeleteViewController : BSMLAutomaticViewController
{
    public event Action<FinishResult>? DidFinish;
    public abstract record FinishResult;
    public sealed record Confirm(TrickConfig ConfigForDeletion) : FinishResult;
    public sealed record Cancel : FinishResult;
    
    private TrickConfig? trickConfigForDeletion;
    
    public void ConfirmPressed()
    {
        DidFinish?.Invoke(trickConfigForDeletion != null ? new Confirm(trickConfigForDeletion) : new Cancel());
    }

    public void CancelPressed()
    {
        DidFinish?.Invoke(new Cancel());
    }

    public void SetConfigForDeletion(TrickConfig config)
    {
        trickConfigForDeletion = config;
    }
}