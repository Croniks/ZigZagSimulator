using UnityEngine;


public enum LevelDifficulty { Easy = 0, Normal = 1, Hard = 2 }
public enum CapsuleRule { RandomFrom5 = 0, InOrder = 1 }


public class SettingsManager : MonoBehaviour
{
    public bool debug = false;

    public static SettingsManager Instance { get; private set; }

    [SerializeField] private LevelDifficulty _levelDifficulty = LevelDifficulty.Hard;
    [SerializeField] private CapsuleRule _capsuleRule = CapsuleRule.InOrder;
    [SerializeField] private LayerMask _layerMaskForNextPlatform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveSpeedMin = 3f;
    [SerializeField] private float _moveSpeedMax = 7f;
    [SerializeField] private int _maxNumberPlatforms = 50;
    [SerializeField] private int _offsetForPlatforms = 30;
    
    void Awake()
    {
        Instance = this;    
    }
    
    public LevelDifficulty GetLevelDifficulty()
    {
        if(debug)
        {
            return _levelDifficulty;
        }
        
        if(PlayerPrefs.HasKey("LevelDifficulty"))
        {
            return (LevelDifficulty)PlayerPrefs.GetInt("LevelDifficulty");
        }
        
        return _levelDifficulty;
    }
    
    public void SetLevelDifficulty(int _levelDifficulty)
    {
        PlayerPrefs.SetInt("LevelDifficulty", _levelDifficulty);
    }

    public CapsuleRule GetCapsuleRule()
    {
        if (debug)
        {
            return _capsuleRule;
        }

        if (PlayerPrefs.HasKey("CapsuleRule"))
        {
            int _capsuleRule = PlayerPrefs.GetInt("CapsuleRule");

            if(_capsuleRule == 0)
            {
                _capsuleRule = Random.Range(1, 3);
            }
            
            return (CapsuleRule)_capsuleRule;
        }

        return _capsuleRule;
    }

    public void SetCapsuleRule(int _capsuleRule)
    {
        PlayerPrefs.SetInt("CapsuleRule", _capsuleRule);
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

    public int GetMaxNumberPlatforms()
    {
        return _maxNumberPlatforms;
    }

    public int GetOffsetForPlatforms()
    {
        return _offsetForPlatforms;
    }

    public LayerMask GetLayerMaskForNextPlatforms()
    {
        return _layerMaskForNextPlatform;
    }
}