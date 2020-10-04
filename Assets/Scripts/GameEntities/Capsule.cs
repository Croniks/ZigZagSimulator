using UnityEngine;


public class Capsule : MonoBehaviour
{
    private UIManager _uiManager;

    void Start()
    {
        _uiManager = UIManager.Instance;
    }
    
    void OnTriggerEnter(Collider other)
    {
        _uiManager.AddScore(1);
        Destroy(gameObject);   
    }
}