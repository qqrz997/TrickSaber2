using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using TrickSaber2.Configuration;
using Zenject;

namespace TrickSaber2.UI.Views;

[HotReload(RelativePathToLayout = @".\Main.bsml")]
[ViewDefinition("TrickSaber2.UI.Views.Main.bsml")]
internal class MainViewController : BSMLAutomaticViewController
{
    [Inject] private readonly PluginConfig pluginConfig = null!;
    [Inject] private readonly LevelCollectionViewController levelCollectionViewController = null!;

    public event Action<FinishResult>? DidFinish;

    public abstract record FinishResult;
    public sealed record Add : FinishResult;
    public sealed record Edit(TrickConfig Config) : FinishResult;
    public sealed record Delete(TrickConfig Config) : FinishResult;
    
    [UIComponent("TrickConfigList")] private readonly CustomCellListTableData customList = null!;
    private TableView trickList = null!;
    private readonly List<CustomListTableData.CustomCellInfo> data = [];
    
    private int selectedConfigIdx;
    
    [UIAction("#post-parse")]
    protected void PostParse()
    {
        trickList = customList.TableView;
        trickList.SetDataSource(new TrickListDataSource(trickList, levelCollectionViewController, data), true);
        Destroy(customList);
    }
    
    public void AddButtonPressed()
    {
        DidFinish?.Invoke(new Add());
    }

    public void EditButtonPressed()
    {
        if (pluginConfig.Tricks.Count > 0)
        {
            DidFinish?.Invoke(new Edit(pluginConfig.Tricks[selectedConfigIdx]));
        }
    }

    public void DeleteButtonPressed()
    {
        DidFinish?.Invoke(new Delete(pluginConfig.Tricks[selectedConfigIdx]));
    }

    public void TrickConfigSelected(TableView tableView, int idx)
    {
        selectedConfigIdx = idx;
    }

    public void RefreshList()
    {
        data.Clear();
        data.AddRange(pluginConfig.Tricks
            .Select(x => $"<color=#fff6aa>{x.Type}</color> - Bound To <color=#fff6aa>{x.InputConfig.Type}")
            .Select(x => new CustomListTableData.CustomCellInfo(x)));
        trickList.ReloadData();
        if (data is [])
        {
            trickList.ClearSelection();
        }
        else
        {
            selectedConfigIdx = Math.Min(selectedConfigIdx, data.Count - 1);
            trickList.SelectCellWithIdx(selectedConfigIdx);
            trickList.ScrollToCellWithIdx(selectedConfigIdx, TableView.ScrollPositionType.End, false);
        }
    }
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        trickList.didSelectCellWithIdxEvent += TrickConfigSelected;
        RefreshList();
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
        base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        trickList.didSelectCellWithIdxEvent -= TrickConfigSelected;
    }

    public bool Enabled
    {
        get => pluginConfig.Enabled;
        set => pluginConfig.Enabled = value;
    }

    public bool TricksAffectHitbox
    {
        get => pluginConfig.TricksAffectHitbox;
        set => pluginConfig.TricksAffectHitbox = value;
    }

    public bool DisableDuringNotes
    {
        get => pluginConfig.DisableDuringNotes;
        set => pluginConfig.DisableDuringNotes = value;
    }
}