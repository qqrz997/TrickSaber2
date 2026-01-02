using System;
using TrickSaber2.Components;
using TrickSaber2.Configuration;
using UnityEngine;

namespace TrickSaber2.Tricks;

internal interface ITrick
{
    /// <summary>
    /// Initializes a trick
    /// </summary>
    /// <param name="timeHelper"></param>
    /// <param name="movementTracker"></param>
    /// <param name="rotationContainer">The transform that the trick will apply additional local rotation to</param>
    /// <returns>The same instance of ITrick with initialized fields</returns>
    ITrick Init(TimeHelper timeHelper, MovementTracker movementTracker, Transform rotationContainer);
    
    void StartTrick(TrickConfig config);
    void StopTrick(Action finishedCallback);
    void CancelTrick();
    void SetInputValue(float value);
}