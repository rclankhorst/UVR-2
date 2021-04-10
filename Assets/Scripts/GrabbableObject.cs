using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class GrabbableObject : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private new Collider collider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        if (!collider.isTrigger)
            Debug.LogWarning("The collider has to be marked as a trigger", collider);
    }
}