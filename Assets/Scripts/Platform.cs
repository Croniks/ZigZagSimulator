using UnityEngine;


public class Platform : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rb;
    private bool _falling = false;
    
    
    void OnTriggerExit(Collider other)
    {
        if(!_falling)
        {
            PlatformCrashes();
            _falling = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void PlatformCrashes()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
    }
}