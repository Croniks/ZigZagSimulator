using UnityEngine;


public class DestroyTrigger : MonoBehaviour
{
    private PlatformManager _platformManager;
    private int _destroyedPlatformsLimit;
    private int _destroyedPlatformsQuantity = 0;


    void Start()
    {
        _platformManager = PlatformManager.Instance;
        _destroyedPlatformsLimit = ((SettingsManager.Instance.GetMaxNumberPlatforms()) / 2);
    }

    void OnTriggerEnter(Collider other)
    {
        _destroyedPlatformsQuantity++;
        if(_destroyedPlatformsQuantity == _destroyedPlatformsLimit)
        {
            _destroyedPlatformsQuantity = 0;
            _platformManager.BuildPlatforms();
        }
    }
}
