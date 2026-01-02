using System;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using TrickSaber2.Configuration;
using Zenject;

namespace TrickSaber2.UI.Views;

[HotReload(RelativePathToLayout = @".\BindingCreation.bsml")]
[ViewDefinition("TrickSaber2.UI.Views.BindingCreation.bsml")]
internal class BindingCreationViewController : BSMLAutomaticViewController
{
    [Inject] private readonly PluginConfig pluginConfig = null!;
    
    public event Action<FinishResult>? DidFinish;
    public abstract record FinishResult;
    public sealed record Success : FinishResult;
    public sealed record Warning(string Message) : FinishResult;
    public sealed record Cancel : FinishResult;
        
    [UIComponent("TrickTypeList")] private readonly ListSetting trickTypeList = null!;
    [UIComponent("InputBindingDropdown")] private readonly DropDownListSetting inputBindingDropdown = null!;
    
    public Array TrickTypeOptions { get; } = Enum.GetValues(typeof(TrickType));
    public Array InputBindingOptions { get; } = Enum.GetValues(typeof(InputType));

    [UIAction("#post-parse")]
    public void PostParse()
    {
        trickTypeList.Value = (TrickType)0;
    }
    
    public void ConfirmPressed()
    {
        var trickType = (TrickType)trickTypeList.Value;
        var inputType = (InputType)inputBindingDropdown.Value;
        var trickConfig = new TrickConfig(trickType, inputType);
        if (pluginConfig.Tricks.Any(trick => trick.InputConfig.Type == inputType))
        {
            DidFinish?.Invoke(new Warning(
                $"Couldn't add binding because a trick with the same binding \"{inputType}\" already exists."));
        }
        else
        {
            pluginConfig.Tricks.Add(trickConfig);
            DidFinish?.Invoke(new Success());
        }
    }

    public void CancelPressed()
    {
        DidFinish?.Invoke(new Cancel());
    }
}