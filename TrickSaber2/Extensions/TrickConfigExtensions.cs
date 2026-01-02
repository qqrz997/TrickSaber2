using System;
using System.Collections.Generic;
using System.Linq;
using TrickSaber2.Configuration;
using TrickSaber2.Models;
using UnityEngine;
using static UnityEngine.XR.CommonUsages;

namespace TrickSaber2.Extensions;

internal static class TrickConfigExtensions
{
    public static IEnumerable<IInputHandler> ToHandlers(this IEnumerable<TrickConfig> tricks) => tricks
        .SelectMany(x => VRDeviceHelper.GetControllers().Select(x.CreateInputHandler))
#if DEBUG
        .Append(new KeyboardInputHandler(KeyCode.Alpha1, tricks.FirstOrDefault() ?? new()))
        .Append(new KeyboardInputHandler(KeyCode.Alpha2, tricks.Skip(1).FirstOrDefault() ?? new()))
#endif
    ;

    public static bool HasTimeWarp(this TrickConfig trick) => 
        trick.Type is TrickType.Throw && !trick.TrickSpecificSettings.ThrowTimeWarpMultiplier.Approx(1f);

    public static bool HaveTimeWarp(this IEnumerable<TrickConfig> tricks)
    {
        foreach (var trick in tricks)
        {
            if (trick.HasTimeWarp())
            {
                return true;
            }
        }
        
        return false;
    }
    
    private static IInputHandler CreateInputHandler(this TrickConfig config, VRDeviceHelper.Controller controller) =>
        config.InputConfig.Type switch
        {
            InputType.Grip => new FloatVRInputHandler(controller, grip, config),
            InputType.GripButton => new BoolVRInputHandler(controller, gripButton, config),
            InputType.MenuButton => new BoolVRInputHandler(controller, menuButton, config),
            InputType.Primary2DAxisX => new JoystickVRInputHandler(controller, primary2DAxis, config, Axis.X),
            InputType.Primary2DAxisY => new JoystickVRInputHandler(controller, primary2DAxis, config, Axis.Y),
            InputType.Primary2DAxisClick => new BoolVRInputHandler(controller, primary2DAxisClick, config),
            InputType.Primary2DAxisTouch => new BoolVRInputHandler(controller, primary2DAxisTouch, config),
            InputType.PrimaryButton => new BoolVRInputHandler(controller, primaryButton, config),
            InputType.PrimaryTouch => new BoolVRInputHandler(controller, primaryTouch, config),
            InputType.Secondary2DAxisClick => new BoolVRInputHandler(controller, secondary2DAxisClick, config),
            InputType.Secondary2DAxisTouch => new BoolVRInputHandler(controller, secondary2DAxisTouch, config),
            InputType.Secondary2DAxisX => new JoystickVRInputHandler(controller, secondary2DAxis, config, Axis.X),
            InputType.Secondary2DAxisY => new JoystickVRInputHandler(controller, secondary2DAxis, config, Axis.Y),
            InputType.SecondaryButton => new BoolVRInputHandler(controller, secondaryButton, config),
            InputType.SecondaryTouch => new BoolVRInputHandler(controller, secondaryTouch, config),
            InputType.Trigger => new FloatVRInputHandler(controller, trigger, config),
            InputType.TriggerButton => new BoolVRInputHandler(controller, triggerButton, config),
            _ => throw new ArgumentOutOfRangeException()
        };
}