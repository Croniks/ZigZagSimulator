using UnityEngine;
using Events;


public enum LevelDifficulty { Easy = 0, Normal = 1, Hard = 2 }
public enum CapsuleRule { RandomFrom5 = 0, InOrder = 1 }


public class SettingsManager : MonoBehaviour
{
    public bool debug = false;

    public static SettingsManager Instance { get; private set; }

    [SerializeField] private LevelDifficulty _levelDifficulty = LevelDifficulty.Hard;
    [SerializeField] private CapsuleRule _capsuleRule = CapsuleRule.InOrder;
    [SerializeField] private float _platformMoveSpeed = 5f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveSpeedMin = 3f;
    [SerializeField] private float _moveSpeedMax = 7f;
    [SerializeField] private float _cameraOffset = 1f;
    [SerializeField] private float _fallingTime = 2f;
    [SerializeField] private float _fallingDistance = 4f;
    [SerializeField] private int _maxNumberPlatforms = 70;
    [SerializeField] private int _numberPlatformsUnderConstruction = 30;
    
    
    void Awake()
    {
        Instance = this;    
    }

    void Start()
    {
        EventAggregator.BallVelocityChangedEvent.Subscribe(SetMoveSpeed);
        EventAggregator.LevelDifficultyChangedEvent.Subscribe(SetLevelDifficulty);
        EventAggregator.CapsuleRuleChangedEvent.Subscribe(SetCapsuleRule);
    }

    void OnApplicationQuit()
    {
        EventAggregator.BallVelocityChangedEvent.Unsubscribe(SetMoveSpeed);
        EventAggregator.LevelDifficultyChangedEvent.Unsubscribe(SetLevelDifficulty);
        EventAggregator.CapsuleRuleChangedEvent.Unsubscribe(SetCapsuleRule);
    }
    
    public LevelDifficulty GetLevelDifficulty()
    {
        if (debug)
        {
            return _levelDifficulty;
        }

        if (PlayerPrefs.HasKey("LevelDifficulty"))
        {
            _levelDifficulty = (LevelDifficulty)PlayerPrefs.GetInt("LevelDifficulty");
        }

        return _levelDifficulty;
    }
    
    public void SetLevelDifficulty(int levelDifficulty)
    {
        PlayerPrefs.SetInt("LevelDifficulty", levelDifficulty);
    }

    public CapsuleRule GetCapsuleRule()
    {
        if (debug)
        {
            return _capsuleRule;
        }

        if (PlayerPrefs.HasKey("CapsuleRule"))
        {
            _capsuleRule = (CapsuleRule)PlayerPrefs.GetInt("CapsuleRule"); 
        }
        
        return _capsuleRule;
    }

    public void SetCapsuleRule(int capsuleRule)
    {
        PlayerPrefs.SetInt("CapsuleRule", capsuleRule);
    }

    public float GetPlatformMoveSpeed()
    {
        if (debug)
        {
            return _platformMoveSpeed;
        }

        if (PlayerPrefs.HasKey("PlatformMoveSpeed"))
        {
            return PlayerPrefs.GetFloat("PlatformMoveSpeed");
        }

        return _platformMoveSpeed;
    }

    public void SetPlatformMoveSpeed(float moveSpeed)
    {
        PlayerPrefs.SetFloat("PlatformMoveSpeed", moveSpeed);
    }

    public float GetMoveSpeed()
    {
        if(debug)
        {
            return _moveSpeed;
        }
        
        if(PlayerPrefs.HasKey("MoveSpeed"))
        {
            return PlayerPrefs.GetFloat("MoveSpeed");
        }
        
        return _moveSpeed;
    }
    
    public void SetMoveSpeed(float moveSpeed)
    {
        PlayerPrefs.SetFloat("MoveSpeed", moveSpeed);
    }

    public float GetMoveSpeedMin()
    {
        return _moveSpeedMin;
    }
    
    public float GetMoveSpeedMax()
    {
        return _moveSpeedMax;
    }

    public float GetCameraOffset()
    {
        return _cameraOffset;
    }

    public float GetFallingTime()
    {
        return _fallingTime;
    }

    public float GetFallingDistance()
    {
        return _fallingDistance;
    }

    public int GetMaxNumberPlatforms()
    {
        return _maxNumberPlatforms;
    }

    public int GetNumberPlatformsUnderConstruction()
    {
        return _numberPlatformsUnderConstruction;
    }
}