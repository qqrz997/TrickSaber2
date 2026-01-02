#if DEBUG
using TrickSaber2.Configuration;
using UnityEngine;

namespace TrickSaber2.Models;

internal class KeyboardInputHandler : IInputHandler
{
    private readonly KeyCode keyCode;

    public KeyboardInputHandler(
        KeyCode keyCode,
        TrickConfig trickConfig)
    {
        this.keyCode = keyCode;
        TrickConfig = trickConfig;
    }
    
    private bool lastValue;
    
    public TrickConfig TrickConfig { get; }
    public bool DeactivatedLastCheck { get; private set; }
    
    public bool CheckInputDidChange()
    {
        var isActive = Input.GetKey(keyCode);
        var didChange = lastValue != isActive;
        lastValue = isActive;
        DeactivatedLastCheck = didChange && !isActive;
        return didChange;
    }

    public float GetInputValue() => lastValue ? 1f : 0f;
}
#endif