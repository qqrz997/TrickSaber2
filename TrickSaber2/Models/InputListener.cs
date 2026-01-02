using System;
using System.Collections.Generic;
using System.Linq;

namespace TrickSaber2.Models;

internal class InputListener
{
    private IInputHandler[] handlers = [];
    
    public event Action<IInputHandler>? InputActivatedEvent;
    public event Action<IInputHandler>? InputDeactivatedEvent;

    public void SetHandlers(IEnumerable<IInputHandler> handlers)
    {
        this.handlers = handlers.ToArray();
    }
    
    public void ManualUpdate()
    {
        foreach (var handler in handlers)
        {
            if (!handler.CheckInputDidChange())
            {
                continue;
            }

            if (handler.DeactivatedLastCheck)
            {
                InputDeactivatedEvent?.Invoke(handler);
            }
            else
            {
                InputActivatedEvent?.Invoke(handler);
            }
        }
    }
}