using System;
using System.Collections;
using TrickSaber2.Components;
using TrickSaber2.Configuration;
using TrickSaber2.Extensions;
using UnityEngine;

namespace TrickSaber2.Tricks;

internal sealed class ThrowTrick : MonoBehaviour, ITrick
{
    private readonly WaitForEndOfFrame waitForEndOfFrame = new();
    private TimeHelper timeHelper = null!;
    private MovementTracker movementTracker = null!;
    private Rigidbody rigidbody = null!;

    public ITrick Init(TimeHelper timeHelper, MovementTracker movementTracker, Transform rotationContainer)
    {
        this.timeHelper = timeHelper;
        this.movementTracker = movementTracker;
        rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = false;
        rigidbody.angularDamping = 0f;
        rigidbody.maxAngularVelocity = float.MaxValue;
        return this;
    }

    private bool isCancelled;
    private float maxReturnTime;
    private float minReturnSpeed;
    private Action? stopTrickFinishedCallback;
    
    public void StartTrick(TrickConfig config)
    {
        var settings = config.TrickSpecificSettings;
        maxReturnTime = settings.ThrowMaxReturnTime;
        minReturnSpeed = settings.ThrowMinReturnSpeed;
        isCancelled = false;
        
        rigidbody.isKinematic = false;
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidbody.position = movementTracker.transform.position;
        rigidbody.rotation = movementTracker.transform.rotation;
     
        var angularVelocity = movementTracker.GetAverageAngularVelocity();
        var velocity = movementTracker.GetAverageVelocity();
        
        // todo: this needs to be replaced with an axis that is always exactly perpendicular to the length of the saber
        movementTracker.transform.rotation.ToAngleAxis(out _, out var axis);
        
        rigidbody.linearVelocity = velocity * settings.ThrowVelocityMultiplier;
        rigidbody.angularVelocity = axis * angularVelocity.magnitude;
    }

    public void SetInputValue(float value) { }

    public void StopTrick(Action finishedCallback)
    {
        stopTrickFinishedCallback = finishedCallback;
        StartCoroutine(DecelerateAndReturnSaber());
    }

    public void CancelTrick()
    {
        StopAllCoroutines();
        HandleEndOfTrick();
    }

    private IEnumerator DecelerateAndReturnSaber()
    {
        var distanceToTarget = Vector3.Distance(rigidbody.position, movementTracker.transform.position);
        var snapDistance = distanceToTarget / (1f / timeHelper.FixedDeltaTime) * 2f;

        if (distanceToTarget < snapDistance)
        {
            HandleEndOfTrick();
            yield break;
        }

        var maxReturnSpeed = distanceToTarget / maxReturnTime;
        var minReturnTime = distanceToTarget / minReturnSpeed;
        var timeToReturn = maxReturnSpeed > minReturnSpeed ? maxReturnTime : minReturnTime;
        var timeToDecelerate = timeToReturn * 0.33f;
        var timeTerminalVelocity = timeToReturn * 0.67f;
        
        var startVelocity = rigidbody.linearVelocity;
        Vector3 targetVelocity = default;

        // Decelerate
        var t = 0f;
        while (t < 1f)
        {
            if (isCancelled) yield break;
            var targetPosition = movementTracker.transform.position;
            distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            var direction = (targetPosition - transform.position).normalized;
            // Recalculate the targetVelocity because the target is moving
            targetVelocity = direction * (distanceToTarget / timeTerminalVelocity);
            rigidbody.linearVelocity = Vector3.Lerp(startVelocity, targetVelocity, t);
            t += timeHelper.DeltaTime / timeToDecelerate;
            yield return waitForEndOfFrame;
        }
        
        rigidbody.linearVelocity = targetVelocity;
        snapDistance = targetVelocity.magnitude * timeHelper.FixedDeltaTime;
        
        // Move towards controller
        while (distanceToTarget > snapDistance)
        {
            if (isCancelled) yield break;
            // Should lerp position / velocity to match the time required to return to target
            var movementVector = (movementTracker.transform.position - rigidbody.position).normalized;
            rigidbody.linearVelocity = movementVector * rigidbody.linearVelocity.magnitude;
            distanceToTarget = Vector3.Distance(rigidbody.position, movementTracker.transform.position);
            yield return waitForEndOfFrame;
        }
        
        transform.position = movementTracker.transform.position;
        HandleEndOfTrick();
    }

    private void HandleEndOfTrick()
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = true;
        rigidbody.interpolation = RigidbodyInterpolation.None;
        stopTrickFinishedCallback?.Invoke();
    }
}