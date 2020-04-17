using UnityEngine;


public enum LevelDifficulty { Easy = 1, Normal = 2, Hard = 4 }
public enum CapsuleRule { Random = 0, RandomFrom5 = 1, InOrder = 2 }


public class SettingsManager : MonoBehaviour
{
    [SerializeField] private LevelDifficulty _levelDifficulty = LevelDifficulty.Hard;
    [SerializeField] private CapsuleRule _capsuleRule = CapsuleRule.InOrder;
    [SerializeField] private LayerMask _layerMaskForNextPlatform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveSpeedMin = 3f;
    [SerializeField] private float _moveSpeedMax = 7f;
    [SerializeField] private int _maxNumberPlatforms = 50;
    [SerializeField] private int _offsetForPlatforms = 30;
    
    
    public LevelDifficulty GetLevelDifficulty(bool debug = false)
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

    public CapsuleRule GetCapsuleRule(bool debug = false)
    {
        if (debug)
        {
            if(_capsuleRule == CapsuleRule.Random)
            {
                return (CapsuleRule)Random.Range(1, 3);
            }

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

    public float GetMoveSpeed(bool debug = false)
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

    public float GetMaxNumberPlatforms()
    {
        return _maxNumberPlatforms;
    }

    public float GetOffsetForPlatforms()
    {
        return _offsetForPlatforms;
    }

    public float GetLayerMaskForNextPlatforms()
    {
        return _layerMaskForNextPlatform;
    }
}