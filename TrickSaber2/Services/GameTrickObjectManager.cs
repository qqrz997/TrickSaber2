using System.Collections.Generic;
using System.Linq;
using TrickSaber2.Components;
using TrickSaber2.Configuration;
using TrickSaber2.Extensions;
using TrickSaber2.Tricks;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber2.Services;

internal class GameTrickObjectManager
{
    private readonly PluginConfig pluginConfig;
    private readonly SaberManager saberManager;
    private readonly TrickObjectContainer.Factory trickObjectContainerFactory;

    public GameTrickObjectManager(
        PluginConfig pluginConfig,
        SaberManager saberManager,
        TrickObjectContainer.Factory trickObjectContainerFactory)
    {
        this.pluginConfig = pluginConfig;
        this.saberManager = saberManager;
        this.trickObjectContainerFactory = trickObjectContainerFactory;
    }

    public IEnumerable<TrickObjectContainer> GetTrickObjectContainers()
    {
        yield return CreateContainerForSaber(saberManager.leftSaber, XRNode.LeftHand);
        yield return CreateContainerForSaber(saberManager.rightSaber, XRNode.RightHand);
    }

    private TrickObjectContainer CreateContainerForSaber(Saber saber, XRNode node)
    {
        var movementTracker = saber.gameObject.GetOrAddComponent<MovementTracker>();
        var trickObjects = GetTrickObjects(saber);
        return trickObjectContainerFactory.Create(new(node, trickObjects.ToArray(), movementTracker));
    }

    private IEnumerable<TrickObject> GetTrickObjects(Saber saber)
    {
        foreach (var saberModel in GetSaberModel(saber))
        {
            yield return saberModel;
        }

        if (pluginConfig.TricksAffectHitbox)
        {
            foreach (var bladeTransform in GetBladeTransforms(saber))
            {
                yield return bladeTransform;
            }
        }

        foreach (var reeSaber in GetReeSaber(saber))
        {
            yield return reeSaber;
        }
    }

    private static IEnumerable<TrickObject> GetSaberModel(Saber saber)
    {
        var smc = saber.transform.GetComponentInChildren<SaberModelController>();
        if (smc != null) yield return smc.gameObject.AddComponent<TrickObject>();
    }

    private static IEnumerable<TrickObject> GetBladeTransforms(Saber saber)
    {
        foreach (Transform child in saber.transform)
        {
            if (child.name is "Top" or "Bottom")
            {
                yield return child.gameObject.AddComponent<TrickObject>();
            }
        }
    }
    
    private static IEnumerable<TrickObject> GetReeSaber(Saber saber)
    {
        foreach (Transform child in saber.transform)
        {
            if (child.name == "ReeSaber")
            {
                yield return child.gameObject.AddComponent<TrickObject>();
                yield break;
            }
        }
    }
}