using UnityEngine;
using System.Collections;


public class Ball : MonoBehaviour
{
    public float ballVelocity = 5;
    public float ballDestroyTime = 1;
    public float camerOffset = 1f;

    [SerializeField]
    private Transform _cameraTransform;
    private Transform _selfTransform;
    private GameManager _gameManager;

    private float _x, _y, _z = 0f;
    private Vector3 _onZ = new Vector3(0f, 0f, 1f);
    private Vector3 _onX = new Vector3(1f, 0f, 0f);
    private Vector3 _cameraDirection = new Vector3(0.5f, 0f, 0.5f);
    private Vector3 _ballDirection;
    private float _cameraDisplacement;
    private bool _isForward = true;

    
    void Start()
    {
        _gameManager = GameManager.Instance;
        _selfTransform = GetComponent<Transform>();
        _cameraTransform = _cameraTransform.GetComponent<Transform>();
        _ballDirection = Vector3.zero;
        _y = _cameraTransform.position.y;
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
        _cameraTransform.position = new Vector3(_cameraDisplacement, _y, _cameraDisplacement);
    }
    
    private void ChangeDirection(bool isForward)
    {
        if(isForward)
        {
            _ballDirection = _onZ;
        }
        else
        {
            _ballDirection = _onX;
        }

        _isForward = isForward;
    }
    
    public void StartMoving()
    {
        _ballDirection = _onZ;
    }

    public void StopMoving()
    {
        _ballDirection = Vector3.zero;
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        _gameManager.EndGame();
        Destroy(gameObject);
    }
    
    //IEnumerator DestroyWithDelay(float sec)
    //{
    //    yield return new WaitForSeconds(sec);
    //    Destroy(gameObject);
    //    yield break;
    //}
}