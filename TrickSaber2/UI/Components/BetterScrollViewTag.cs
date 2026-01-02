using System;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Tags;
using HMUI;
using UnityEngine;
using UnityEngine.UI;
using VRUIControls;
using Object = UnityEngine.Object;

namespace TrickSaber2.UI.Components;

public class BetterScrollViewTag : BSMLTag
{
    public override string[] Aliases => ["better-scroll-view"];
    
    public override GameObject CreateObject(Transform parent)
    {
        var template = GetScrollViewTemplate();
        
        // ScrollView
        var scrollView = DiContainer.InstantiatePrefab(template, parent).GetComponent<ScrollView>();
        scrollView.gameObject.AddComponent<ScrollViewLateUpdater>();
        var scrollViewLayoutElement = scrollView.gameObject.AddComponent<LayoutElement>();
        var scrollViewContentSizeFitter = scrollView.gameObject.AddComponent<ContentSizeFitter>();
        scrollViewContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Scroll Bar
        var scrollBar = (RectTransform)scrollView.transform.Find("ScrollBar");
        scrollBar.offsetMin = new(10f, 0f);
        scrollBar.offsetMax = new(0f, 0f);
        
        var upButton = (RectTransform)scrollView._pageUpButton.transform;
        upButton.anchorMin = new(0f, 0f);
        upButton.anchorMax = new(0.5f, 1f);
        var upButtonIcon = (RectTransform)upButton.Find("Icon");
        upButtonIcon.offsetMin = upButtonIcon.offsetMin with { x = -1.25f };
        upButtonIcon.offsetMax = upButtonIcon.offsetMax with { x = 1.25f };
        
        var downButton = (RectTransform)scrollView._pageDownButton.transform;
        downButton.anchorMin = new(0f, 0f);
        downButton.anchorMax = new(0.5f, 0.5f);
        var downButtonIcon = (RectTransform)downButton.Find("Icon");
        downButtonIcon.offsetMin = downButtonIcon.offsetMin with { x = -1.25f };
        downButtonIcon.offsetMax = downButtonIcon.offsetMax with { x = 1.25f };
        
        // View Port
        var viewPort = scrollView.viewportTransform;
        viewPort.offsetMin = new(0f, 0f);
        viewPort.offsetMax = new(0f, 0f);
        
        // Content
        var content = scrollView.contentTransform;
        foreach (Transform t in content)
        {
            Object.Destroy(t.gameObject);
        }
        content.GetComponent<ContentSizeFitter>().enabled = true;
        
        var externalComponents = content.gameObject.AddComponent<ExternalComponents>();
        externalComponents.Components.Add(scrollView);
        externalComponents.Components.Add(scrollView.transform);
        externalComponents.Components.Add(scrollViewLayoutElement);
        
        return content.gameObject;
    }

    private ScrollView? scrollViewTemplate;
    private ScrollView GetScrollViewTemplate()
    {
        if (scrollViewTemplate != null)
        {
            return scrollViewTemplate;
        }
        
        var searchFilterParamsViewController = DiContainer.TryResolve<SearchFilterParamsViewController>();
        if (searchFilterParamsViewController == null)
        {
            throw new NullReferenceException($"Unable to resolve {nameof(SearchFilterParamsViewController)}");
        }

        scrollViewTemplate = searchFilterParamsViewController.GetComponentInChildren<ScrollView>();
        if (scrollViewTemplate == null)
        {
            throw new NullReferenceException($"Unable to find ScrollView template");
        }
        
        return scrollViewTemplate;
    }
}