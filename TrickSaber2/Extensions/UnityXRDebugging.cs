#if DEBUG
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using IPA.Utilities;
using UnityEngine.XR;

namespace TrickSaber2.Extensions;

internal static class UnityXRDebugging
{
    public static void OutputDeviceFeatures(this XRNode xrNode)
    {
        var inputDevice = InputDevices.GetDeviceAtXRNode(xrNode);
        if (!inputDevice.isValid)
        {
            Plugin.Log.Warn($"Could not find device for {xrNode}");
            return;
        }
        
        var inputFeatures = new List<InputFeatureUsage>();
        if (!inputDevice.TryGetFeatureUsages(inputFeatures))
        {
            Plugin.Log.Warn($"Could not find features for {xrNode}");
            return;
        }

        var stringBuilder = new StringBuilder();
        stringBuilder
            .AppendLine($"Displaying features for {xrNode}")
            .AppendLine();
        inputFeatures
            .OrderBy(f => f.type.Name)
            .ThenBy(f => f.name)
            .Select(f => $"{f.name} [{f.type.Name}]")
            .ForEach(line => stringBuilder.AppendLine(line));
        
        var path = Path.Combine(UnityGame.InstallPath, $"{nameof(OutputDeviceFeatures)}_{xrNode}.txt");
        File.WriteAllText(path, stringBuilder.ToString());
        Process.Start(path);
    }
}
#endif