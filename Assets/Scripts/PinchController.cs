using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controller which tracks pinching of the current hand it is attached to. 
/// </summary>
[RequireComponent(typeof(OVRHand))]
public class PinchController : MonoBehaviour
{
    /// <summary>
    /// Finger which is responsible for grab motion.
    /// </summary>
    [SerializeField] private OVRHand.HandFinger triggerFinger = OVRHand.HandFinger.Index;

    /// <summary>
    /// Colliders from the current <see cref="triggerFinger"/>. Found when a pinch is detected.
    /// </summary>
    private CapsuleCollider[] fingerColliders;
    
    /// <summary>
    /// Components which keep track of which items we're colliding triggers with. Added to the finger's colliders.
    /// </summary>
    private PinchCollisionTracker[] trackers;

    /// <summary>
    /// Keeping track of the objects we're currently holding.
    /// </summary>
    private readonly Dictionary<GrabbableObject, Transform> heldObjects = new Dictionary<GrabbableObject, Transform>();

    #region Private members

    private OVRHand hand;
    private bool previousPinchState;
    private bool pinchState;

    #endregion


    private void Awake()
    {
        hand = GetComponent<OVRHand>();
    }

    private void Update()
    {
        // No hand? No update.
        if (!hand.IsTracked)
            return;

        // Set pinch to 'true' if the index finger is touching the palm
        pinchState = hand.GetFingerIsPinching(triggerFinger);

        // If we already recorded a pinch had started, don't trigger anything
        if (pinchState == previousPinchState)
            return;

        // If pinching, call pinch start
        if (pinchState)
            OnPinchStart();

        // Else call pinch end
        else OnPinchEnd();

        // Store the previous value so it doesn't call too often
        previousPinchState = pinchState;
    }

    /// <summary>
    /// Called when a pinch has first been detected.
    /// </summary>
    private void OnPinchStart()
    {
        // Record the colliders associated to the triggerFinger
        if (fingerColliders == null || fingerColliders.Length == 0)
        {
            fingerColliders = GetComponentsInChildren<CapsuleCollider>()
                .Where(c => c.gameObject.name.Contains($"{triggerFinger}")).ToArray();
            trackers = fingerColliders.Select(c => c.gameObject.AddComponent<PinchCollisionTracker>()).ToArray();
        }

        foreach (PinchCollisionTracker tracker in trackers)
            if (tracker.GrabbableObject != null)
                if (!heldObjects.ContainsKey(tracker.GrabbableObject))
                    heldObjects.Add(tracker.GrabbableObject, tracker.GrabbableObject.transform.parent);

        Debug.Log($"To hold {heldObjects.Count} objects");

        foreach (KeyValuePair<GrabbableObject, Transform> keyValuePair in heldObjects)
            keyValuePair.Key.transform.SetParent(transform);
    }

    /// <summary>
    /// Called when the pinch has let go.
    /// </summary>
    private void OnPinchEnd()
    {
        Debug.Log($"To release {heldObjects.Count} objects");
            
        // Reset the object to its old parent, while retaining the world position
        foreach (KeyValuePair<GrabbableObject, Transform> keyValuePair in heldObjects)
        {
            GrabbableObject key = keyValuePair.Key;
            key.transform.SetParent(keyValuePair.Value, true);
            
            // Oh and also lol it's no longer not moving.
            if (key.TryGetComponent(out Rigidbody rb))
                rb.isKinematic = false;
        }

        // Clean up the list
        heldObjects.Clear();
    }
}