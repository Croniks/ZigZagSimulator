using UnityEngine;
using System.Collections.Generic;
using Events;


public class PlatformBuilder : MonoBehaviour
{
    private PlatformManager _platformManager;
    private SettingsManager _settingsManager;
    private List<GameObject> _platformsPool;
    private float _displacement;
    private Vector3 _lastPlatformPosition; 
    private LevelDifficulty _levelDifficulty;
    private CapsuleRule _capsuleRule;


    void Start()
    {
        _platformManager = PlatformManager.Instance;

        _levelDifficulty = _settingsManager.GetLevelDifficulty();
        _capsuleRule = _settingsManager.GetCapsuleRule();
        //_platforms[_platforms.Count - 1].transform.position 

        EventAggregator.LevelDifficultyChangedEvent.Subscribe(ChangeLevelDifficulty);
        EventAggregator.CapsuleRuleChangedEvent.Subscribe(ChangeCapsuleRule);
    }

    public void ChangeLevelDifficulty(int index)
    {
        _levelDifficulty = (LevelDifficulty)index;
    }

    public void ChangeCapsuleRule(int index)
    {
        _capsuleRule = (CapsuleRule)index;
    }

    public void BuildPlatforms(int maxPlatforms)
    {
        //_platformsPool.Add(Instantiate(_generalPlatformPrefab, _generalPlatformPrefab.transform.position, Quaternion.identity));
        GameObject platform = null;
        Vector3 postion;

        GameObject[] platformsForCaplsule = new GameObject[5];

        for (int i = 0; i < maxPlatforms; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                postion = NextPosition(_displacement, true);
            }
            else
            {
                postion = NextPosition(_displacement, false);
            }

            _lastPlatformPosition = postion;
            platform = Instantiate(_platformManager.currentPlatformPrefab, postion, Quaternion.identity);
            _platformsPool.Add(platform);

            platformsForCaplsule[i % 5] = platform;

            if ((i + 1) % 5 == 0)
            {
                CreateCapsules(platformsForCaplsule, i);
            }
        }
    }

    private Vector3 NextPosition(float displacement, bool onX)
    {
        Vector3 newPosition = _lastPlatformPosition;

        if (onX)
        {
            newPosition.x += displacement;
        }
        else
        {
            newPosition.z += displacement;
        }

        return newPosition;
    }

    private void CreateCapsules(GameObject[] platforms, int quantityPlatforms)
    {
        GameObject platformForCapsule = null;
        GameObject capsule = null;

        if (_capsuleRule == CapsuleRule.RandomFrom5)
        {
            platformForCapsule = platforms[Random.Range(0, 5)];
        }
        else
        {
            platformForCapsule = platforms[(quantityPlatforms / 5) % 5];
        }

        Vector3 pos = new Vector3(platformForCapsule.transform.position.x,
                                    _platformManager.capsule.transform.position.y,
                                                platformForCapsule.transform.position.z);

        capsule = Instantiate(_platformManager.capsule, pos, Quaternion.identity);
        capsule.transform.parent = platformForCapsule.transform;
    }

    private void DeletePlatforms()
    {
        foreach (var platform in _platformsPool)
        {
            Destroy(platform);
        }

        _platformsPool.Clear();
    }

    private void ApplySettings(LevelDifficulty levelDifficulty)
    {
        _currentPlatformPrefab = _platformPrefabs[(int)levelDifficulty];
        _currentStartPlatform = _startPlatforms[(int)levelDifficulty];

        _boxSizes = new Vector3(_currentPlatformPrefab.transform.localScale.x / 2,
                                   _currentPlatformPrefab.transform.localScale.y / 2,
                                       _currentPlatformPrefab.transform.localScale.z / 2);

        _displacement = _currentPlatformPrefab.transform.localScale.x;
        _lastPlatformPosition = _currentStartPlatform.position;
    }
}
