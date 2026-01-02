using System.Collections.Generic;
using HMUI;
using UnityEngine;
using static BeatSaberMarkupLanguage.Components.CustomListTableData;

namespace TrickSaber2.UI;

internal class TrickListDataSource : TableView.IDataSource
{
    private readonly TableView trickList;
    private readonly LevelCollectionViewController levelCollectionViewController;
    private readonly List<CustomCellInfo> data;

    public TrickListDataSource(
        TableView trickList,
        LevelCollectionViewController levelCollectionViewController,
        List<CustomCellInfo> data)
    {
        this.trickList = trickList;
        this.levelCollectionViewController = levelCollectionViewController;
        this.data = data;
    }
    
    private LevelListTableCell? tableCellPrefab;
    
    public float CellSize(int idx) => 6.5f;
    public int NumberOfCells() => data.Count;

    public TableCell CellForIdx(TableView tableView, int idx)
    {
        var cell = (LevelListTableCell)trickList.DequeueReusableCellForIdentifier("TrickListTableCell");

        if (cell == null)
        {
            if (tableCellPrefab == null) tableCellPrefab = CreateTableCellPrefab();
            cell = Object.Instantiate(tableCellPrefab);
            cell.reuseIdentifier = "TrickListTableCell";
        }

        var info = data[idx];
        cell._songNameText.text = info.Text;
        
        return cell;
    }

    private LevelListTableCell CreateTableCellPrefab()
    {
        var gameObject = Object.Instantiate(levelCollectionViewController.transform
            .Find("LevelsTableView/TableView/Viewport/Content/LevelListTableCell").gameObject);
        gameObject.name = "TrickListCell";
        var cell = gameObject.GetComponent<LevelListTableCell>();
        var textTransform = cell._songNameText.rectTransform;
        textTransform.offsetMin = new(1.25f, -3.25f);
        textTransform.offsetMax = new(-1.25f, 3.25f);
        Object.DestroyImmediate(cell._songAuthorText.gameObject);
        Object.DestroyImmediate(cell._coverImage.gameObject);
        Object.DestroyImmediate(cell._promoBadgeGo);
        Object.DestroyImmediate(cell._updatedBadgeGo);
        Object.DestroyImmediate(cell._favoritesBadgeImage.gameObject);
        Object.DestroyImmediate(cell._songBpmText.gameObject);
        Object.DestroyImmediate(cell._songDurationText.gameObject);
        Object.DestroyImmediate(cell.transform.Find("BpmIcon").gameObject);
        return cell;
    }
}