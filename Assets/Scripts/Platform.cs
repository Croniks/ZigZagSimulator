using UnityEngine;


public class Platform : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField]
    private Rigidbody _rb;
    private bool _falling = false;

    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    void OnTriggerExit(Collider other)
    {
        if(!_falling)
        {
            PlatformCrashes();
            _falling = true;
        }
        else
        {
            _gameManager.platformsPool.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    
    private void PlatformCrashes()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
    }
}