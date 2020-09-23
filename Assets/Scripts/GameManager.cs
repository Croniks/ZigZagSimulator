using UnityEngine;
using System.Collections.Generic;
using Events;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private LevelDifficulty _levelDifficulty;
    private CapsuleRule _capsuleRule;

    private LayerMask _layerMaskForNextPlatform;
    
    private SettingsManager _settingsManager;
    private UIManager _uiManager;

    [SerializeField] private BallController _ball;
    [SerializeField] private CameraController _camera;
    [SerializeField] private GameObject _capsule;
    
    [SerializeField] private GameObject _generalPlatformPrefab;
    [SerializeField] private GameObject[] _platformPrefabs;
    [SerializeField] private Transform[] _startPlatforms;
    
    private GameObject _currentPlatformPrefab;
    private Transform _currentStartPlatform;

    private List<GameObject> _platformsPool;

    private Vector3 _boxSizes;
    private float _displacement;
    private Vector3 _lastPlatformPosition;

    
    void Awake()
    {
        Instance = this;

        EventAggregator.BallVelocityChangedEvent = new BallVelocityChangedEvent();
        EventAggregator.LevelDifficultyChangedEvent = new LevelDifficultyChangedEvent();
        EventAggregator.CapsuleRuleChangedEvent = new CapsuleRuleChangedEvent();
        EventAggregator.GameOverEvent = new GameOverEvent();
    }
    
    void Start()
    {
        _platformsPool = new List<GameObject>();
        _settingsManager = SettingsManager.Instance;
        _uiManager = UIManager.Instance;
       
        EventAggregator.BallVelocityChangedEvent.Subscribe(ChangeMoveSpeed);
        EventAggregator.LevelDifficultyChangedEvent.Subscribe(ChangeLevelDifficulty);
        EventAggregator.CapsuleRuleChangedEvent.Subscribe(ChangeCapsuleRule);
        EventAggregator.GameOverEvent.Subscribe(FinishGame);

        SetSettings();
    }
    
    void OnApplicationQuit()
    {
        EventAggregator.BallVelocityChangedEvent.Unsubscribe(ChangeMoveSpeed);
        EventAggregator.LevelDifficultyChangedEvent.Unsubscribe(ChangeLevelDifficulty);
        EventAggregator.CapsuleRuleChangedEvent.Unsubscribe(ChangeCapsuleRule);
    }

    public void SetSettings()
    {
        _layerMaskForNextPlatform = _settingsManager.GetLayerMaskForNextPlatforms();
        
        _levelDifficulty = _settingsManager.GetLevelDifficulty();
        _capsuleRule = _settingsManager.GetCapsuleRule();

        _ball.ChangeVelocity(_settingsManager.GetMoveSpeed());

        _uiManager.SetMoveSpeedToUI(
            _uiManager.TransformRealMoveSpeedToUIValue(_settingsManager.GetMoveSpeedMin(),
                                                         _settingsManager.GetMoveSpeedMax(),
                                                             _settingsManager.GetMoveSpeed())
        );
        
        _uiManager.SetLevelDifficultyToUI(_levelDifficulty);
        _uiManager.SetCapsuleRuleToUI(_capsuleRule);
    }

    public void ChangeLevelDifficulty(int index)
    {
        _levelDifficulty = (LevelDifficulty)index;
    }

    public void ChangeCapsuleRule(int index)
    {
        _capsuleRule = (CapsuleRule)index;
    }
    
    public void ChangeMoveSpeed(float moveSpeed)
    {
        _ball.ChangeVelocity(moveSpeed);
    }

    public void PreStartGame()
    {
        _camera.enabled = true;
        _camera.MoveToStartingPosition();

        DeletePlatforms();
        ApplySettings(_levelDifficulty);
        BuildPlatforms(_settingsManager.GetMaxNumberPlatforms() + _settingsManager.GetOffsetForPlatforms());
    }

    public void StartGame()
    {
        _ball.enabled = true;
        _ball.StartMoving();
    }
    
    public void FinishGame()
    {
        _camera.enabled = false;
        _ball.StopMoving();
        //_ball.DropBall(_settingsManager.GetFallingTime(),
        //                _settingsManager.GetFallingDistance());
        _ball.MoveToStartingPosition();
        _ball.enabled = false;
    }
    
    public void RestartGame()
    {
        DeletePlatforms();
        StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
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

    public void BuildPlatforms(int maxPlatforms)
    {
        //_platformsPool.Add(Instantiate(_generalPlatformPrefab, _generalPlatformPrefab.transform.position, Quaternion.identity));
        GameObject platform = null;
        Vector3 postion;
        
        GameObject[] platformsForCaplsule = new GameObject[5];
        
        for (int i=0; i < maxPlatforms; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                postion = NextPosition(_displacement, true);
                if (!IsConstructionAllowed(postion))
                    postion = NextPosition(_displacement, false);
            }
            else
            {
                postion = NextPosition(_displacement, false);
                if (!IsConstructionAllowed(postion))
                    postion = NextPosition(_displacement, true);
            }
            
            _lastPlatformPosition = postion;
            platform = Instantiate(_currentPlatformPrefab, postion, Quaternion.identity);
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
    
    private bool IsConstructionAllowed(Vector3 postion)
    {
        Collider[] hitColliders = Physics.OverlapBox(postion, _boxSizes, Quaternion.identity, _layerMaskForNextPlatform);

        return !(hitColliders.Length > 0);
    }
    
    private void CreateCapsules(GameObject[] platforms, int quantityPlatforms)
    {
        GameObject platformForCapsule = null;
        GameObject capsule = null;

        if(_capsuleRule == CapsuleRule.RandomFrom5)
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
}