using System;
using UnityEngine;
using UnityEngine.UI;
using Events;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private ToggleGroup _levelDifficulty;
    [SerializeField] private ToggleGroup _capsuleRule;
    [SerializeField] private Slider _moveSpeed;
    [SerializeField] private Text _score;
    [SerializeField] private UIPanel _startPanel;
    [SerializeField] private RectTransform _backgroundMainPanel;
    
    void Awake()
    {
        Instance = this; 
    }

    void Start()
    {
        SetMoveSpeedToUI(
            TransformRealMoveSpeedToUIValue(SettingsManager.Instance.GetMoveSpeedMin(),
                                                SettingsManager.Instance.GetMoveSpeedMax(),
                                                    SettingsManager.Instance.GetMoveSpeed())
        );
        
        SetLevelDifficultyToUI(SettingsManager.Instance.GetLevelDifficulty());
        SetCapsuleRuleToUI(SettingsManager.Instance.GetCapsuleRule());

        EventAggregator.GameOverEvent.Subscribe(GameOver);
    }
    
    public void InitiateBallVelocityChangedEvent()
    {
        var moveSpeed = TransformUIValueToRealMoveSpeed(
            SettingsManager.Instance.GetMoveSpeedMin(),
                SettingsManager.Instance.GetMoveSpeedMax(),
                                                _moveSpeed.value
        );

        EventAggregator.BallVelocityChangedEvent?.Publish(moveSpeed);
    }
    
    public void InitiateCapsuleRuleChangedEvent(int capsuleRule)
    {
        EventAggregator.CapsuleRuleChangedEvent?.Publish(capsuleRule);
    }
    
    public void InitiatLevelDifficultyChangedEvent(int levelDifficulty)
    {
        EventAggregator.LevelDifficultyChangedEvent?.Publish(levelDifficulty);
    }
    
    public void GameOver()
    {
        _startPanel.GetComponent<UIPanel>().IsOpened = true;
        _backgroundMainPanel.gameObject.SetActive(true);
    }

    public void SetMoveSpeedToUI(float value)
    {
        _moveSpeed.value = value;
    }
    
    public float TransformRealMoveSpeedToUIValue(float speedMin, float speedMax, float speed)
    {
        if (speedMin >= speedMax)
        {
            throw new Exception("The minimum speed is greater than or equal to the maximum!");
        }
        
        speed = Mathf.Clamp(speed, speedMin, speedMax);
        return (((speed - speedMin) / (speedMax - speedMin)) * _moveSpeed.maxValue);
    }

    public float TransformUIValueToRealMoveSpeed(float speedMin, float speedMax, float value)
    {
        return speedMin + (((speedMax - speedMin) * value) / _moveSpeed.maxValue);
    }

    public void SetLevelDifficultyToUI(LevelDifficulty levelDifficulty)
    {
        _levelDifficulty.GetComponentsInChildren<Toggle>()[(int)levelDifficulty].isOn = true;
    }
    
    public void SetCapsuleRuleToUI(CapsuleRule capsuleRule)
    {
        _capsuleRule.GetComponentsInChildren<Toggle>()[(int)capsuleRule].isOn = true;
    }
    
    public void SetScore(int number)
    {
        _score.text = number.ToString();
    }
}