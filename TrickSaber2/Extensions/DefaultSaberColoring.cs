using UnityEngine;

namespace TrickSaber2.Extensions;

internal static class DefaultSaberColoring
{
    public static void SetNewColor(this SaberModelController smc, Color color)
    {
        foreach (var setSaberGlowColor in smc._setSaberGlowColors) setSaberGlowColor.SetNewColor(color);
        foreach (var setSaberFakeGlowColor in smc._setSaberFakeGlowColors) setSaberFakeGlowColor.SetNewColor(color);
        if (smc._saberLight != null) smc._saberLight.color = color;
    }

    private static void SetNewColor(this SetSaberGlowColor setSaberGlowColor, Color color)
    {
        var materialPropertyBlock = setSaberGlowColor._materialPropertyBlock ?? new MaterialPropertyBlock();

        foreach (var pair in setSaberGlowColor._propertyTintColorPairs)
        {
            materialPropertyBlock.SetColor(pair.property, color * pair.tintColor);
        }

        setSaberGlowColor._meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    private static void SetNewColor(this SetSaberFakeGlowColor setSaberFakeGlowColor, Color color)
    {
        setSaberFakeGlowColor._parametric3SliceSprite.color = color;
        setSaberFakeGlowColor._parametric3SliceSprite.Refresh();
    }
}