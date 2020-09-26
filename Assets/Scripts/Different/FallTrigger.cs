using UnityEngine;


public class FallTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Platform>().DeletePlatform();    
    }
}