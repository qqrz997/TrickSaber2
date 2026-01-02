using System;
using System.Collections;
using TrickSaber2.Components;
using TrickSaber2.Configuration;
using TrickSaber2.Extensions;
using UnityEngine;

namespace TrickSaber2.Tricks;

internal sealed class SpinTrick : MonoBehaviour, ITrick
{
    private readonly WaitForEndOfFrame waitForEndOfFrame = new();
    private TimeHelper timeHelper = null!;
    private MovementTracker movementTracker = null!;
    private Transform rotationContainer = null!;
    
    public ITrick Init(TimeHelper timeHelper, MovementTracker movementTracker, Transform rotationContainer)
    {
        this.timeHelper = timeHelper;
        this.movementTracker = movementTracker;
        this.rotationContainer = rotationContainer;
        return this;
    }

    private Action? stopTrickFinishedCallback;
    private TrickSpecificSettings settings = new();
    private bool toggle;
    private float inputValue = 1f; // between 0 and 1
    private float spinSpeed;
    private Vector3 spinDirection;
    
    public void StartTrick(TrickConfig config)
    {
        settings = config.TrickSpecificSettings;
        toggle = config.InputConfig.Toggle;
        enabled = true;

        if (!settings.SpinVelocityBased)
        {
            spinSpeed = 20f;
            spinDirection = Vector3.right;
        }
        else
        {
            var angularVelocity = movementTracker.GetAverageAngularVelocity();
            spinSpeed = angularVelocity.magnitude;
            spinDirection = angularVelocity.GetClosestAxis();
        }

        spinSpeed *= settings.SpinSpeedMultiplier;
    }

    public void SetInputValue(float value)
    {
        inputValue = toggle ? 1f : value;
    }

    public void StopTrick(Action finisedCallback)
    {
        stopTrickFinishedCallback = finisedCallback;
        enabled = false;
        StartCoroutine(CompleteRotation());
    }

    public void CancelTrick()
    {
        StopAllCoroutines();
        HandleEndOfTrick();
    }

    private void LateUpdate()
    {
        // inputValue can vary during the spin if using an analogue input method 
        rotationContainer.Rotate(spinDirection * (spinSpeed * inputValue));
        
        // todo: rotating the whole TrickObject does mean rotating in the same direction of the spin
        // which can lead to the saber spinning faster or slower when you rotate the controller
        // there should be a way to make the object rotate without affecting the spin but for now this is passable

        var currentPos = transform.position;
        var targetPos = movementTracker.transform.position;
        transform.position = settings.SpinPositionSmoothing.Approx(0f) ? targetPos
            : GetSmoothedPos(currentPos, targetPos, timeHelper.DeltaTime, settings.SpinPositionSmoothing);
        
        var currentRot = transform.rotation;
        var targetRot = movementTracker.transform.rotation;
        transform.rotation = settings.SpinRotationSmoothing.Approx(0f) ? targetRot
            : GetSmoothedRot(currentRot, targetRot, timeHelper.DeltaTime, settings.SpinRotationSmoothing);
        
        static Vector3 GetSmoothedPos(Vector3 current, Vector3 target, float deltaTime, float smoothing) =>
            Vector3.Lerp(current, target, Mathf.Clamp(deltaTime / smoothing, 0f, 1f));
        static Quaternion GetSmoothedRot(Quaternion current, Quaternion target, float deltaTime, float smoothing) =>
            Quaternion.Lerp(current, target, Mathf.Clamp(deltaTime / smoothing, 0f, 1f));
    }
    
    private IEnumerator CompleteRotation()
    {
        const int minSpeed = 3;

        var speed = Mathf.Abs(spinSpeed) >= minSpeed ? spinSpeed
            : Mathf.Sign(spinSpeed) * minSpeed;

        var threshold = Mathf.Abs(speed) + 0.1f;
        var angle = Quaternion.Angle(rotationContainer.rotation, Quaternion.identity);
        
        while (angle > threshold)
        {
            rotationContainer.rotation = Quaternion.Euler(spinDirection * speed);
            angle = Quaternion.Angle(rotationContainer.rotation, Quaternion.identity);
            yield return waitForEndOfFrame;
        }

        HandleEndOfTrick();
    }

    private void HandleEndOfTrick()
    {
        enabled = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rotationContainer.localPosition = Vector3.zero;
        rotationContainer.localRotation = Quaternion.identity;
        stopTrickFinishedCallback?.Invoke();
    }
}