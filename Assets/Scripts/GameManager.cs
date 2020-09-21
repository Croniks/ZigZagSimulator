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

    [SerializeField] private Ball _ball;
    [SerializeField] private GameObject _capsule;
    
    [SerializeField] private GameObject _generalPlatformPrefab;
    [SerializeField] private GameObject[] _platformPrefabs;
    [SerializeField] private Transform[] _startPlatforms;
    
    private GameObject _generalPlatform;
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
    
    public void StartGame()
    {
        ApplySettings(_levelDifficulty);
        BuildPlatforms(_settingsManager.GetMaxNumberPlatforms() + _settingsManager.GetOffsetForPlatforms());
        
        _ball.StartMoving();
    }
    
    public void FinishGame()
    {
        
    }
    
    public void RestartGame()
    {
        DeletePlatforms();
       
        _ball.enabled = false;
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

    //build a level according to the difficulty level
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
        if(_generalPlatform != null)
        {
            Destroy(_generalPlatform);
        }

        _generalPlatform = Instantiate(_generalPlatformPrefab, _generalPlatformPrefab.transform.position, Quaternion.identity);
        GameObject platform = null;
        Vector3 postion;

        GameObject[] CollectionforCaplsule = new GameObject[5];
        
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

            CollectionforCaplsule[i % 5] = platform;
            
            //if ((i + 1) % 5 == 0)
            //{
            //    CreateCapsules(CollectionforCaplsule);
            //}
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
    
    private void CreateCapsules(GameObject[] gameObjects)
    {
        GameObject gameObjForCapsule = null;
        GameObject capsule = null;

        if(_capsuleRule == CapsuleRule.RandomFrom5)
        {
            gameObjForCapsule = gameObjects[Random.Range(0, 4)];
        }
        else
        {
            //gameObjForCapsule = gameObjects[_n];
            //_n++;
            //if(_n > 4) _n = 0;
        }
        
        Vector3 pos = new Vector3(gameObjForCapsule.transform.position.x, 
                                                    _capsule.transform.position.y, 
                                                        gameObjForCapsule.transform.position.z);

        capsule = Instantiate(_capsule, pos, Quaternion.identity);
        capsule.transform.parent = gameObjForCapsule.transform;
    }
}