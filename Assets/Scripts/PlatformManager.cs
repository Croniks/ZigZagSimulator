using UnityEngine;
using Events;

public enum Option { Level, Capsule }

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;
    
    [SerializeField] private PlatformBuilder[] _platformBuilders;
    private Transform[] _platformBuilderTransforms;
    private int _buildersQuantity;
    public GameObject[] startingPlatforms;
    public GameObject[] platformPrefabs;
    public GameObject capsule;

    private Vector3 _lastPlatformPostion;
    

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _buildersQuantity = _platformBuilders.Length;

        //_startingPlatformBuilderTransform = _startingPlatformBuilder.GetComponent<Transform>();
        //_anotherPlatformBuilderTransform = _anotherPlatformBuilder.GetComponent<Transform>();
        //_lastPlatformPostion = 

        EventAggregator.LevelDifficultyChangedEvent.Subscribe(ChangeLevelDifficulty);
        EventAggregator.CapsuleRuleChangedEvent.Subscribe(ChangeCapsuleRule);
    }
    
    void Update()
    {
        
    }

    private void TakeBuilderTransforms()
    {
        for(int i=0; i < _buildersQuantity; i++)
        {
            _platformBuilderTransforms[i] = _platformBuilders[i].transform;
        }
    }

    public void SetOptionToBuilders(Option option, int value)
    {
        for (int i = 0; i < _buildersQuantity; i++)
        {
            _platformBuilders[i].SetOption(option, value);
        }
    }

    
    public void ChangeLevelDifficulty(int value)
    {
        SetOptionToBuilders(Option.Level, value);
        //_levelDifficulty = (LevelDifficulty)index;
        //ApplySettings();
        //CalculateDisplacementAndBorderNumber();
    }

    public void ChangeCapsuleRule(int index)
    {
        //_capsuleRule = (CapsuleRule)index;
        //ApplySettings();
        //CalculateDisplacementAndBorderNumber();
    }


    private void SetLastPlatformPositon(PlatformBuilder platformBuilder, Vector3 position)
    {
        platformBuilder.SetLastPlatformPosition(position);
    }
}