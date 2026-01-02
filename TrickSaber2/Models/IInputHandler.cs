using TrickSaber2.Configuration;

namespace TrickSaber2.Models;

internal interface IInputHandler
{
    public TrickConfig TrickConfig { get; }
    public bool DeactivatedLastCheck { get; }
    public bool CheckInputDidChange();
    float GetInputValue();
}