using UnityEngine;


public class PlatformDestroyTrigger : MonoBehaviour
{
    [SerializeField] GameObject _platform; 

    void OnTriggerEnter(Collider other)
    {
        DestroyPlatform();
    }
    
    public void DestroyPlatform()
    {
        Destroy(_platform);
    }
}