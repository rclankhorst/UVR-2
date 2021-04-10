using UnityEngine;

public class PinchCollisionTracker : MonoBehaviour
{
    [SerializeField] private GrabbableObject grabbableObject;

    public GrabbableObject GrabbableObject => grabbableObject;

    private void OnTriggerEnter(Collider other)
    {
        // When the trigger reports it's colliding with something, check if it has our own component
        // This will not assign the object if it fails to find it.
        other.gameObject.TryGetComponent(out grabbableObject);
    }

    private void OnTriggerExit(Collider other)
    {
        // No collision, reset the value.
        grabbableObject = null;
    }
}