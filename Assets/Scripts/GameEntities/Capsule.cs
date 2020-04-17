using UnityEngine;


public class Capsule : MonoBehaviour
{
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.Instance;
    }
    
    void OnTriggerEnter(Collider other)
    {
        //_gameManager.NumberOfPoints = 1;
        Destroy(gameObject);   
    }
}