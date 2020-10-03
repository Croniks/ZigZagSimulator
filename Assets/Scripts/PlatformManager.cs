using UnityEngine;


public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;
    
    [SerializeField] private PlatformBuilder _platformBuilder;
    private Transform _platformBuilderTransform;
    public GameObject[] platformPrefabs;
    public GameObject[] startingPlatforms;
    public GameObject capsule;
    public int numberPlatformsUnderConstruction;
    private Vector3 _direction = Vector3.back;
    private float _platformMoveSpeed;

      
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _platformMoveSpeed = SettingsManager.Instance.GetPlatformMoveSpeed();
        _platformBuilderTransform = _platformBuilder.GetComponent<Transform>();
        numberPlatformsUnderConstruction = SettingsManager.Instance.GetNumberPlatformsUnderConstruction();

        _platformBuilder.Init();
        _platformBuilder.Build();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _platformBuilder.UpdateLastPlatformPosition();
            _platformBuilder.Build();
        }

        MoveBuilder();
    }
    
    private void MoveBuilder()
    {
        _platformBuilderTransform.Translate(_direction * Time.deltaTime * _platformMoveSpeed, Space.World);
    }
}