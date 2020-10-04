using UnityEngine;
using System.Collections;


public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;
    
    [SerializeField] private PlatformBuilder _platformBuilder;
    [SerializeField] private GameObject _generalPlatformPrefab;
    private GameObject _generalPlatform;
    private Transform _platformBuilderTransform;
    private Vector3 _generalPlatformStartingPosition;
    public GameObject[] platformPrefabs;
    public GameObject[] startingPlatforms;
    public GameObject capsule;
    private Vector3 _direction = Vector3.back;
    private float _platformMoveSpeed;
    private int _maxPlatforms;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _platformMoveSpeed = SettingsManager.Instance.GetPlatformMoveSpeed();
        _platformBuilderTransform = _platformBuilder.GetComponent<Transform>();
        _generalPlatformStartingPosition = _generalPlatformPrefab.transform.position;
        _maxPlatforms = SettingsManager.Instance.GetMaxNumberPlatforms();
    }
    
    public void BuildLevel()
    {
        _generalPlatform = Instantiate(_generalPlatformPrefab, 
                                        _generalPlatformStartingPosition,
                                                Quaternion.Euler(0f, 45f, 0), 
                                                        _platformBuilderTransform);
        _platformBuilder.Init();
        _platformBuilder.Build(_maxPlatforms);
    }

    public void BuildPlatforms()
    {
        _platformBuilder.UpdateLastPlatformPosition();
        _platformBuilder.Build(_maxPlatforms / 2);
    }

    public void DeletePlatforms()
    {
        Destroy(_generalPlatform);
        _platformBuilder.DestroyPlatforms();
    }

    public void StartMoving()
    {
        StartCoroutine(MoveBuilder());
    }

    public void StopMoving()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveBuilder()
    {
        while (true)
        {
            _platformBuilderTransform.Translate(_direction * Time.deltaTime * _platformMoveSpeed, Space.World);
            yield return null;
        }
    }
}