using System;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using TrickSaber2.Configuration;
using TrickSaber2.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TrickSaber2.UI.Views;

[HotReload(RelativePathToLayout = @".\TrickEditor.bsml")]
[ViewDefinition("TrickSaber2.UI.Views.TrickEditor.bsml")]
internal class TrickEditorViewController : BSMLAutomaticViewController
{
    [Inject] private readonly PluginConfig pluginConfig = null!;
    [Inject] private readonly MenuSabersManager menuSabersManager = null!;
    
    [UIComponent("SpinSettingsContainer")] private readonly ScrollView spinSettingsContainer = null!;
    [UIComponent("ThrowSettingsContainer")] private readonly ScrollView throwSettingsContainer = null!;

    [UIAction("#post-parse")]
    public void PostParse()
    {
    }
    
    private TrickConfig configForEditing = new();
    private readonly TrickConfig initialConfig = new();

    public Array TrickTypeOptions { get; } = Enum.GetValues(typeof(TrickType));
    public Array InputBindingOptions { get; } = Enum.GetValues(typeof(InputType));
    
    public event Action<FinishResult>? DidFinish;
    public abstract record FinishResult;
    public sealed record Warning(string Message) : FinishResult;
    public sealed record Finished : FinishResult;

    public void SetConfigForEditing(TrickConfig trickConfig)
    {
        configForEditing = trickConfig;
        configForEditing.CopyTo(initialConfig);
    }

    public void ApplyButtonPressed()
    {
        DidFinish?.Invoke(new Finished());
    }

    public void CancelButtonPressed()
    {
        initialConfig.CopyTo(configForEditing);
        DidFinish?.Invoke(new Finished());
    }

    public TrickType TrickType
    {
        get => configForEditing.Type;
        set => HandleTrickTypeChanged(value);
    }

    public InputType InputType
    {
        get => configForEditing.InputConfig.Type;
        set => HandleInputTypeChanged(value);
    }

    public float ActivationThreshold
    {
        get => configForEditing.InputConfig.ActivationThreshold;
        set => configForEditing.InputConfig.ActivationThreshold = value;
    }
    
    public bool ActivateOnRelease
    {
        get => configForEditing.InputConfig.ActivateOnRelease;
        set => configForEditing.InputConfig.ActivateOnRelease = value;
    }

    public bool Toggle
    {
        get => configForEditing.InputConfig.Toggle;
        set => configForEditing.InputConfig.Toggle = value;
    }
    
    public float ActivationDelay
    {
        get => configForEditing.InputConfig.ActivationDelay;
        set => configForEditing.InputConfig.ActivationDelay = value;
    }
    
    public float DeactivationDelay
    {
        get => configForEditing.InputConfig.DeactivationDelay;
        set => configForEditing.InputConfig.DeactivationDelay = value;
    }

    public bool SpinVelocityBased
    {
        get => configForEditing.TrickSpecificSettings.SpinVelocityBased;
        set => configForEditing.TrickSpecificSettings.SpinVelocityBased = value;
    }

    public float SpinSpeedMultiplier
    {
        get => configForEditing.TrickSpecificSettings.SpinSpeedMultiplier;
        set => configForEditing.TrickSpecificSettings.SpinSpeedMultiplier = value;
    }

    public float SpinPositionSmoothing
    {
        get => configForEditing.TrickSpecificSettings.SpinPositionSmoothing;
        set => configForEditing.TrickSpecificSettings.SpinPositionSmoothing = value;
    }

    public float SpinRotationSmoothing
    {
        get => configForEditing.TrickSpecificSettings.SpinRotationSmoothing;
        set => configForEditing.TrickSpecificSettings.SpinRotationSmoothing = value;
    }

    public float ThrowVelocityMultiplier
    {
        get => configForEditing.TrickSpecificSettings.ThrowVelocityMultiplier;
        set => configForEditing.TrickSpecificSettings.ThrowVelocityMultiplier = value;
    }

    public float ThrowMaxReturnTime
    {
        get => configForEditing.TrickSpecificSettings.ThrowMaxReturnTime;
        set => configForEditing.TrickSpecificSettings.ThrowMaxReturnTime = value;
    }

    public float ThrowMinReturnSpeed
    {
        get => configForEditing.TrickSpecificSettings.ThrowMinReturnSpeed;
        set => configForEditing.TrickSpecificSettings.ThrowMinReturnSpeed = value;
    }

    public float ThrowTimeWarpMultiplier
    {
        get => configForEditing.TrickSpecificSettings.ThrowTimeWarpMultiplier;
        set => configForEditing.TrickSpecificSettings.ThrowTimeWarpMultiplier = value;
    }

    public float ThrowTimeWarpTransitionTime
    {
        get => configForEditing.TrickSpecificSettings.ThrowTimeWarpTransitionTime;
        set => configForEditing.TrickSpecificSettings.ThrowTimeWarpTransitionTime = value;
    }

    public string PercentValueFormatter(float val) => $"{val:P0}";
    public string SecondsValueFormatter(float val) => $"{val:F1} s";
    public string SpeedValueFormatter(float val) => $"{val:F1} m/s";

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        NotifyPropertyChanged(null);
        HandleTrickTypeChanged(configForEditing.Type);
    }

    private void HandleTrickTypeChanged(TrickType type)
    {
        configForEditing.Type = type;
        spinSettingsContainer.gameObject.SetActive(type is TrickType.Spin);
        throwSettingsContainer.gameObject.SetActive(type is TrickType.Throw);
    }

    private void HandleInputTypeChanged(InputType type)
    {
        if (configForEditing.InputConfig.Type == type)
        {
            return;
        }
        if (pluginConfig.Tricks.Any(x => x.InputConfig.Type == type))
        {
            DidFinish?.Invoke(new Warning($"A trick is already using the binding \"{type}\". Please select a different binding."));
            return;
        }
        configForEditing.InputConfig.Type = type;
        menuSabersManager.UpdateInputListeners();
    }
}