using UnityEngine;


public class PlatformBuilder : MonoBehaviour
{ 
    private GameObject[] _platformsPool;
    private GameObject _platform;
    private GameObject _capsule;
    private Transform _selfTransform;
    private float _displacement;
    private float _rightBorderNumber;
    private float _leftBorderNumber;
    private LevelDifficulty _levelDifficulty;
    private CapsuleRule _capsuleRule;
    private int _maxPlatforms;
    private Vector3 _lastPlatformPosition;
    private Vector3 _nextPosition;
    GameObject[] _platformsForCaplsule = new GameObject[5];


    void Start()
    {
        _selfTransform = GetComponent<Transform>();
        _maxPlatforms = SettingsManager.Instance.GetMaxNumberPlatforms();
        _platformsPool = new GameObject[_maxPlatforms];
    }
    
    public void Build()
    {
        BuildPlatforms();
    }
    
    public void Destroy()
    {
        DestroyPlatforms();
    }

    public void SetLastPlatformPosition(Vector3 position)
    {
        _lastPlatformPosition = position;
    }

    public void SetPlatform(GameObject platform)
    {
        _platform = platform;
    }

    public void SetCapsule(GameObject capsule)
    {
        _capsule = capsule;
    }

    public void SetOption(Option option, int value)
    {
        if(option == Option.Level)
            ChangeLevelDifficulty((LevelDifficulty)value);
        if(option == Option.Capsule)
            ChangeCapsuleRule((CapsuleRule)value);
    }
    
    private void ChangeLevelDifficulty(LevelDifficulty levelDifficulty)
    {
        _levelDifficulty = levelDifficulty;
        CalculateDisplacementAndBorderNumber();
    }
    
    private void ChangeCapsuleRule(CapsuleRule capsuleRule)
    {
        _capsuleRule = capsuleRule;
    }
    
    private void CalculateDisplacementAndBorderNumber()
    {
        if((int)_levelDifficulty == 0)
        { 
            _displacement =  2.1213f;
            _rightBorderNumber = 6.5f;
            _leftBorderNumber = -6.5f;
        }
        else if((int)_levelDifficulty == 1)
        {
            _displacement = 1.4142f;
            _rightBorderNumber = 7.2f;
            _leftBorderNumber = -7.2f;
        } 
        else if((int)_levelDifficulty == 2)
        {
            _displacement = 0.7072f;
            _rightBorderNumber = 7.9f;
            _leftBorderNumber = -7.9f;
        }
    }
    
    public void BuildPlatforms()
    {
        GameObject platform = null;
        Vector3 postion;
        
        for (int i = 0; i < _maxPlatforms; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                postion = NextPosition(true);
            }
            else
            {
                postion = NextPosition(false);
            }

            _lastPlatformPosition = postion;
            platform = Instantiate(_platform, postion, Quaternion.Euler(0f, 45f, 0));
            _platformsPool[i] = platform;
            platform.transform.parent = _selfTransform;

             _platformsForCaplsule[i % 5] = platform;

            if ((i + 1) % 5 == 0)
            {
                CreateCapsules(_platformsForCaplsule, i);
            }
        }
    }
    
    private Vector3 NextPosition(bool IsGoRight)
    {
        Vector3 _nextPosition = _lastPlatformPosition;

        _nextPosition.z += _displacement;
        
        if(IsGoRight)
        {
            _nextPosition.x += _displacement;

            if(_nextPosition.x > _rightBorderNumber)
                _nextPosition.x -= _displacement * 2;
        }
        else
        {
            _nextPosition.x -= _displacement;

            if (_nextPosition.x < _leftBorderNumber)
                _nextPosition.x += _displacement * 2;
        }
        
        return _nextPosition;
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