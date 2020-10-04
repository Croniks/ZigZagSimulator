using UnityEngine;
using Events;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private BallController _ballController;
    [SerializeField] private PlatformManager _platformManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Rigidbody _rbBall;
    

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
        EventAggregator.GameOverEvent.Subscribe(FinishGame);
    }
    
    public void PreStartGame()
    {
        _uiManager.ResetScores();
        _platformManager.DeletePlatforms();
        _platformManager.BuildLevel();
        _ballController.gameObject.SetActive(true);
        _ballController.MoveToStartingPosition();
    }

    public void StartGame()
    {
        _platformManager.StartMoving();
        _ballController.enabled = true;
        _ballController.StartMoving();
    }
    
    public void FinishGame()
    {
        _platformManager.StopMoving();
        _ballController.StopMoving();
        _ballController.enabled = false;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }  
}