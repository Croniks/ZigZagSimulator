using UnityEngine;
using Events;


public class BallController : MonoBehaviour
{
    private float _ballVelocity = 5f;
    private Transform _selfTransform;
    private Vector3 _startingPosition;
    private float _selfY;
    private Vector3 _ballDirection = Vector3.zero;
    private bool _isRight = true;

    
    void Start()
    {
        _ballVelocity = SettingsManager.Instance.GetMoveSpeed();
        _selfTransform = GetComponent<Transform>();
        _startingPosition = _selfTransform.position;
        _selfY = _selfTransform.position.y;
        enabled = false;

        EventAggregator.BallVelocityChangedEvent.Subscribe(ChangeVelocity);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            EventAggregator.GameOverEvent.Publish();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            ChangeDirection(!_isRight);
        }
        
        _selfTransform.Translate(_ballDirection * Time.deltaTime * _ballVelocity, Space.World);
    }
    
    void LateUpdate()
    {
        if ((_selfY - _selfTransform.position.y) > 0.5f)
        {
            EventAggregator.GameOverEvent.Publish();
        }
    }

    public void StartMoving()
    { 
        _ballDirection = Vector3.right;
    }

    public void StopMoving()
    {
        _ballDirection = Vector3.zero;
    }

    public void DropBall(float fallingTime, float fallingDistance)
    {
        float fallingHightStepDistance = fallingDistance
                                                / (fallingTime / Time.deltaTime);
        float y = 0f;

        while(true)
        {
            if(fallingTime <= 0)
                return;

            y = _selfTransform.position.y - fallingHightStepDistance;

           fallingTime -= Time.deltaTime;
            _selfTransform.Translate(new Vector3(_selfTransform.position.x,
                                                                            y,
                                                      _selfTransform.position.z), Space.Self);
        }
    }

    public void MoveToStartingPosition()
    {
        _selfTransform.position = _startingPosition;
    }
    
    public void ChangeVelocity(float velocity)
    {
        _ballVelocity = velocity;
    }

    private void ChangeDirection(bool flag)
    {
        if (flag)
        {
            _ballDirection = Vector3.right; 
        }
        else
        {
            _ballDirection = Vector3.left; 
        }
        
        _isRight = flag;
    }
}