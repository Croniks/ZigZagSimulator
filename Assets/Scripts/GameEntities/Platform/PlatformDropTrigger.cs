using UnityEngine;


public class PlatformDropTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    void OnTriggerEnter(Collider other)
    {
        DropPlatform();
    }

    public void DropPlatform()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
    }
}