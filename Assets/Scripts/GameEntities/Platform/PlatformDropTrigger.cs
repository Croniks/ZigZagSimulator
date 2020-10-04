using UnityEngine;


public class PlatformDropTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    void OnTriggerExit(Collider other)
    {
        DropPlatform();
    }

    public void DropPlatform()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
    }
}