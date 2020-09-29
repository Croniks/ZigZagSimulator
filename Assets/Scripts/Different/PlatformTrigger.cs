using UnityEngine;


public class PlatformTrigger : MonoBehaviour
{
    private GameManager _gameManager;
    private int _maxNumberPlatforms;
    private int _currentNumberPlatforms;
    
    
    void Start()
    {
        _gameManager = GameManager.Instance;
        _maxNumberPlatforms = _currentNumberPlatforms = SettingsManager.Instance.GetMaxNumberPlatforms();
    }

    void OnTriggerExit(Collider other)
    {
        other.GetComponent<Platform>().FallPlatform();
        _currentNumberPlatforms -= 1;
        
        if (_currentNumberPlatforms < 1)
        { 
            //_gameManager.BuildPlatforms(_maxNumberPlatforms);
            _currentNumberPlatforms = _maxNumberPlatforms;
        }
    }
}