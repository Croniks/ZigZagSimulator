using UnityEngine;
using Events;


public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    private float _ballVelocity = 5f;
    private Transform _selfTransform;
    private Vector3 _startingPosition;
    private float _selfY;
    private Vector3 _ballDirection = Vector3.zero;
    private bool _isRight = true;

    void Awake()
    {
        _selfTransform = GetComponent<Transform>();
        _startingPosition = _selfTransform.position;
        _selfY = _selfTransform.position.y;
        _rb.Sleep();
    }

    void Start()
    {
        _ballVelocity = SettingsManager.Instance.GetMoveSpeed();
        EventAggregator.BallVelocityChangedEvent.Subscribe(ChangeVelocity);
        enabled = false;
    }
    
    void Update()
    { 
        if(Input.GetKeyDown(KeyCode.F))
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
    
    public void MoveToStartingPosition()
    {
        _selfTransform.position = _startingPosition;
        _selfTransform.eulerAngles = new Vector3(0f, 0f, 0f);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
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