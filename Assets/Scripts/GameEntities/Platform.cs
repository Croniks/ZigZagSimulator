using UnityEngine;


public class Platform : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField]
    private Rigidbody _rb;
   
    
    void Start()
    {
        _gameManager = GameManager.Instance;
    }
    
    public void FallPlatform()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
    }

    public void DeletePlatform()
    {
        //_gameManager.platformsPool.Remove(gameObject);
        Destroy(gameObject);
    }


}