using System.Collections;
using HMUI;
using UnityEngine;

namespace TrickSaber2.UI.Components;

internal class BetterScrollIndicator : VerticalScrollIndicator
{
    private new void RefreshHandle()
    {
        float num = ((RectTransform)base.transform).rect.size.y - 2f * _padding;
        var height = _normalizedPageHeight * num;
        Plugin.Log.Info($"Setting height to {height}");
        _handle.sizeDelta = new(0f, Mathf.Max(height, 2f));
        _handle.anchoredPosition = new(0f, (0f - _progress) * (1f - _normalizedPageHeight) * num - _padding);
    }
}

internal class ScrollViewLateUpdater : MonoBehaviour
{
    private ScrollView scrollView = null!;
    private Coroutine? updateLayoutCoroutine;

    private void Awake()
    {
        scrollView = GetComponent<ScrollView>();
    }
    
    private void OnRectTransformDimensionsChange()
    {
        if (scrollView.isActiveAndEnabled && updateLayoutCoroutine == null)
        {
            updateLayoutCoroutine = StartCoroutine(UpdateLayoutCoroutine());
        }
    }

    private IEnumerator UpdateLayoutCoroutine()
    {
        yield return null;
        scrollView.UpdateContentSize();
        scrollView.RefreshButtons();
        updateLayoutCoroutine = null;
    }
}