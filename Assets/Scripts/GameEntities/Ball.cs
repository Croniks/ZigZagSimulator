using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    private float _ballVelocity;
    
    private float _camerOffset = 1f;
    private GameManager _gameManager;
    private Transform _selfTransform;
    private float _cameraY;
    private float _selfY;
    private Vector3 _ballDirection = Vector3.right;
    private bool _isForward = true;

    
    void Start()
    {
        _gameManager = GameManager.Instance;
        _cameraTransform = _cameraTransform.GetComponent<Transform>();
        _selfTransform = GetComponent<Transform>();
        _selfY = _selfTransform.position.y;
        _cameraY = _cameraTransform.position.y;
        enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ChangeDirection(!_isForward);
        }
        
        _selfTransform.Translate(_ballDirection * Time.deltaTime * _ballVelocity, Space.World);
        _cameraTransform.position = CalculateCameraPosition();
    }

    void LateUpdate()
    {
        if ((_selfY - _selfTransform.position.y) > 0.5f)
        {
            _gameManager.EndGame();
            gameObject.SetActive(false);
        }
    }
    
    // При каждом включении, значения параметров шарика заменяются новыми значениями
    // из настроек GameManager'a
    void OnEnable()
    {
        
    }
    
    private Vector3 CalculateCameraPosition()
    {
        float x = _selfTransform.position.x - _camerOffset;
        float z = _selfTransform.position.z - _camerOffset;
        float cameraDisplacement = (x + z) / 2;

        return new Vector3(cameraDisplacement, _cameraY, cameraDisplacement);
    }

    private void ChangeDirection(bool isForward)
    {
        if(isForward)
        {
            _ballDirection = Vector3.right; // (1, 0, 0)
        }
        else
        {
            _ballDirection = Vector3.forward; // (0, 0, 1)
        }
        
        _isForward = isForward;
    }
    
    public void StopMoving()
    {
        _ballDirection = Vector3.zero;
    }

    public void ChangeVelocity(float velocity)
    {
        _ballVelocity = velocity;
    }
}