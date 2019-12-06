using UnityEngine;


public class Ball : MonoBehaviour
{
    public float ballVelocity = 5;
    public float camerOffset = 1f;

    [SerializeField]
    private Transform _cameraTransform;
    private Transform _selfTransform;
    private float _selfY;
    private GameManager _gameManager;

    private float _x, _y, _z = 0f;
    private Vector3 _onZ = new Vector3(0f, 0f, 1f);
    private Vector3 _onX = new Vector3(1f, 0f, 0f);
    private Vector3 _ballDirection;
    private float _cameraDisplacement;
    private bool _isForward = true;
    private Vector3 _newPosition = new Vector3();

    
    void Start()
    {
        _gameManager = GameManager.Instance;
        _selfTransform = GetComponent<Transform>();
        _cameraTransform = _cameraTransform.GetComponent<Transform>();
        _ballDirection = _onX;
        _selfY = _selfTransform.position.y;
        _y = _cameraTransform.position.y;
        enabled = false;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ChangeDirection(!_isForward);
        }
        
        _selfTransform.Translate(_ballDirection * Time.deltaTime * ballVelocity, Space.World);

        _x = _selfTransform.position.x - camerOffset;
        _z = _selfTransform.position.z - camerOffset;
        
        _cameraDisplacement = (_x + _z) / 2;

        _newPosition.x = _cameraDisplacement;
        _newPosition.y = _y;
        _newPosition.z = _cameraDisplacement;
       
        _cameraTransform.position = _newPosition;
    }

    void LateUpdate()
    {
        if ((_selfY - _selfTransform.position.y) > 0.5f)
        {
            _gameManager.EndGame();
            gameObject.SetActive(false);
        }
    }

    
    private void ChangeDirection(bool isForward)
    {
        if(isForward)
        {
            _ballDirection = _onX;
        }
        else
        {
            _ballDirection = _onZ;
        }

        _isForward = isForward;
    }
    
    public void StopMoving()
    {
        _ballDirection = Vector3.zero;
    }
}