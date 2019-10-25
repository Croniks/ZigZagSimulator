using UnityEngine;


public class Capsule : MonoBehaviour
{
    private GameManager _gameManager;


    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            _gameManager.NumberOfPoints = 1;
            Destroy(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }
}