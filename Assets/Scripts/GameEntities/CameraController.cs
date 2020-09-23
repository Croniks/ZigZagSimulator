using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private BallController _ballController;
    private Transform _ballTransform;
    private Transform _selfTransform;
    private float _cameraOffset = 1f;
    private float _selfY;
    private Vector3 _startingPosition;


    void Start()
    {
        _ballTransform = _ballController.GetComponent<Transform>();
        _selfTransform = transform.GetComponent<Transform>();
        _selfY = _selfTransform.position.y;
        _cameraOffset = SettingsManager.Instance.GetCameraOffset();
        _startingPosition = CalculateCameraPosition();
    }

    void Update()
    {
        _selfTransform.position = CalculateCameraPosition();
    }

    private Vector3 CalculateCameraPosition()
    {
        float x = _ballTransform.position.x - _cameraOffset;
        float z = _ballTransform.position.z - _cameraOffset;
        float cameraDisplacement = (x + z) / 2;

        return new Vector3(cameraDisplacement, _selfY, cameraDisplacement);
    }

    public void MoveToStartingPosition()
    {
        _selfTransform.position = _startingPosition;
    }
}