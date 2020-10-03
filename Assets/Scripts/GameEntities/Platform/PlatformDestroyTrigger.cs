using UnityEngine;


public class PlatformDestroyTrigger : MonoBehaviour
{
    [SerializeField] GameObject _platform; 

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        DestroyPlatform();
    }
    
    public void DestroyPlatform()
    {
        Destroy(_platform);
    }
}