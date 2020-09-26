using UnityEngine;
using Events;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private BallController _ballController;
    
    
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
    
    //void OnApplicationQuit()
    //{
    //    EventAggregator.BallVelocityChangedEvent.Unsubscribe(ChangeMoveSpeed);
    //}
    
    public void PreStartGame()
    {
        
    }

    public void StartGame()
    {
        _ballController.enabled = true;
        _ballController.StartMoving();
    }
    
    public void FinishGame()
    {
        _ballController.StopMoving();
        //_ball.DropBall(_settingsManager.GetFallingTime(),
        //                _settingsManager.GetFallingDistance());
        _ballController.MoveToStartingPosition();
        _ballController.enabled = false;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }  
}