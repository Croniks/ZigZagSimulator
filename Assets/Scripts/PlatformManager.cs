using UnityEngine;
using Events;


public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;
    private SettingsManager _settingsManager;

    [SerializeField] private PlatformBuilder startingPlatformBuilder;     
    [SerializeField] private PlatformBuilder anotherPlatformBuilder;
    [SerializeField] private PlatformBuilder startingPlatformBuilderPrefab;
    public Platform[] startingPlatforms;
    public Platform[] platformPrefabs;
    public GameObject capsule;
    public GameObject currentPlatformPrefab;
    public Transform currentStartPlatform;

    private Vector3 _startingPlatformBuilderPosition;
    private Transform _startingPlatformBuilderTransform;
    private Transform _anotherPlatformBuilderTransform;
    

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _settingsManager = SettingsManager.Instance;

        _levelDifficulty = _settingsManager.GetLevelDifficulty();
        _capsuleRule = _settingsManager.GetCapsuleRule();

        EventAggregator.LevelDifficultyChangedEvent.Subscribe(ChangeLevelDifficulty);
        EventAggregator.CapsuleRuleChangedEvent.Subscribe(ChangeCapsuleRule);
    }
    
    void Update()
    {
        
    }

    public void ChangeLevelDifficulty(int index)
    {
        _levelDifficulty = (LevelDifficulty)index;
    }
    
    public void ChangeCapsuleRule(int index)
    {
        _capsuleRule = (CapsuleRule)index;
    }
}