using System.Collections.Generic;
using UnityEngine;
using Events;


public class PlatformBuilder : MonoBehaviour
{ 
    public List<GameObject> _platformsPool;
    private GameObject _platform;
    private GameObject _capsule;
    private Transform _selfTransform;
    private float _displacement;
    private float _rightBorderNumber;
    private float _leftBorderNumber;
    private LevelDifficulty _levelDifficulty;
    private CapsuleRule _capsuleRule;
    private int _maxPlatforms;
    private Vector3 _nextPlatformPosition;
    GameObject[] _platformsForCaplsule = new GameObject[5];
    

    public void Init()
    {
        _capsuleRule = SettingsManager.Instance.GetCapsuleRule();
        _levelDifficulty = SettingsManager.Instance.GetLevelDifficulty();
        _capsule = PlatformManager.Instance.capsule;
        ChoosePlatform((int)_levelDifficulty);
        CalculateDisplacementAndBorderNumber();
        _selfTransform = GetComponent<Transform>();
        _maxPlatforms = SettingsManager.Instance.GetMaxNumberPlatforms();
        _platformsPool = new List<GameObject>();
        _nextPlatformPosition = PlatformManager.Instance.startingPlatforms[(int)_levelDifficulty].transform.position;
        
        EventAggregator.LevelDifficultyChangedEvent.Subscribe(ChangeLevelDifficulty);
        EventAggregator.CapsuleRuleChangedEvent.Subscribe(ChangeCapsuleRule);
    }
    
    public void ChangeLevelDifficulty(int value)
    {
        _levelDifficulty = (LevelDifficulty)value;
        ChoosePlatform(value);
        CalculateDisplacementAndBorderNumber();
    }

    public void ChangeCapsuleRule(int value)
    {
        _capsuleRule = (CapsuleRule)value;        
    }
     
    public void Build()
    {
        BuildPlatforms();
    }
    
    private void ChoosePlatform(int value)
    {
        _platform = PlatformManager.Instance.platformPrefabs[value];
    }
     
    private void CalculateDisplacementAndBorderNumber()
    {
        if(_levelDifficulty == LevelDifficulty.Easy)
        { 
            _displacement =  2.1213f;
            _rightBorderNumber = 6.5f;
            _leftBorderNumber = -6.5f;
        }
        else if(_levelDifficulty == LevelDifficulty.Normal)
        {
            _displacement = 1.4142f;
            _rightBorderNumber = 7.2f;
            _leftBorderNumber = -7.2f;
        } 
        else if(_levelDifficulty == LevelDifficulty.Hard)
        {
            _displacement = 0.7072f;
            _rightBorderNumber = 7.9f;
            _leftBorderNumber = -7.9f;
        }
    }

    public void UpdateLastPlatformPosition()
    {
        _nextPlatformPosition = _platformsPool[_platformsPool.Count-1].transform.position;
    }

    public void BuildPlatforms()
    {
        GameObject platform = null;
         
        for (int i = 0; i < _maxPlatforms; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                SetNextPlatformPosition(true);
            }
            else
            {
                SetNextPlatformPosition(false);
            }
            
            platform = Instantiate(_platform, _nextPlatformPosition, Quaternion.Euler(0f, 45f, 0), _selfTransform);
            //_platformsPool[i] = platform;
            _platformsPool.Add(platform);
             _platformsForCaplsule[i % 5] = platform;

            if ((i + 1) % 5 == 0)
            {
                CreateCapsules(_platformsForCaplsule, i);
            }
        }
    }
    
    private void SetNextPlatformPosition(bool IsGoRight)
    {
        _nextPlatformPosition.z += _displacement;
        
        if(IsGoRight)
        {
            _nextPlatformPosition.x += _displacement;

            if(_nextPlatformPosition.x > _rightBorderNumber)
                _nextPlatformPosition.x -= _displacement * 2;
        }
        else
        {
            _nextPlatformPosition.x -= _displacement;

            if (_nextPlatformPosition.x < _leftBorderNumber)
                _nextPlatformPosition.x += _displacement * 2;
        }
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
                                                _capsule.transform.position.y,
                                                    platformForCapsule.transform.position.z);

        capsule = Instantiate(_capsule, pos, Quaternion.identity);
        capsule.transform.parent = platformForCapsule.transform;
    }
    
    private void DestroyPlatforms()
    {
        foreach (var platform in _platformsPool)
        {
            Destroy(platform);
        }
    }
}