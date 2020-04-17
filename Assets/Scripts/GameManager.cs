using UnityEngine;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private LevelDifficulty _levelDifficulty;
    private CapsuleRule _capsuleRule;
    private LayerMask _layerMaskForNextPlatform;
    private int _maxNumberPlatforms;
    private int _offsetForPlatforms;
    private float _moveSpeed;
    
    [SerializeField] private Ball _ball;
    private List<GameObject> _platformsPool;

    [SerializeField] private GameObject _generalPlatformPrefab;
    private GameObject _generalPlatform;
    [SerializeField] private GameObject[] _platformPrefabs;
    [SerializeField] private Transform[] _startPlatforms;
    [SerializeField] private GameObject _capsule;
    private GameObject _currentPlatformPrefab;
    private Transform _currentStartPlatform;

    private Vector3 _boxSizes;
    private float _displacement;
    private Vector3 _lastPlatformPosition;
    
    private int _numberOfPoints;
    private int _n = 0; 

    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        _platformsPool = new List<GameObject>();

        // Запрашивать информацию о настройках при старте сцены
        //foreach (Toggle toggle in _levelDifficultyOption.ActiveToggles())
        //{
        //    Debug.Log(toggle);
        //    toggle.isOn = true;
        //}


        SetLevelDifficulty(2);
       
        
        //_startPanel.IsOpened = true;
        //_backgroundStartPanel.SetActive(true);
    }

    public void StartGame()
    { 
        _numberOfPoints = 0;

        DefineLevelDifficulty(_levelDifficulty);
        //BuildPlatforms(_maxNumberPlatforms + _offSetForPlatforms);
        
        _ball.gameObject.SetActive(true);
    }
    
    public void StartMoving()
    {
        _ball.enabled = true;
    }

    public void AcivateStartPanel(bool flag)
    {
        //_startPanel.IsOpened = flag;
        //_backgroundStartPanel.SetActive(flag);
    }

    public void EndGame()
    {
        //_startPanel.IsOpened = true;
        //_backgroundStartPanel.SetActive(true);
    }
    
    public void RestartGame()
    {
        // Удалить предыдущие платформы
        foreach(var platform in _platformsPool)
        {
            Destroy(platform);
        }
        
        _platformsPool.Clear();
        
        _ball.enabled = false;
        StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void SetLevelDifficulty(int index)
    {
        _levelDifficulty = (LevelDifficulty)index;
    }
 
    private void DefineLevelDifficulty(LevelDifficulty levelDifficulty)
    {
        _currentPlatformPrefab = _platformPrefabs[(int)levelDifficulty];
        _currentStartPlatform = _startPlatforms[(int)levelDifficulty];
        
        _boxSizes = new Vector3(_currentPlatformPrefab.transform.localScale.x / 2,
                                   _currentPlatformPrefab.transform.localScale.y / 2,
                                       _currentPlatformPrefab.transform.localScale.z / 2);

        _displacement = _currentPlatformPrefab.transform.localScale.x;
        _lastPlatformPosition = _currentStartPlatform.position;
    }

    public void BuildPlatforms(int maxPlatforms)
    {
        if(_generalPlatform != null)
        {
            Destroy(_generalPlatform);
        }

        _generalPlatform = Instantiate(_generalPlatformPrefab, _generalPlatformPrefab.transform.position, Quaternion.identity);
        GameObject platform = null;
        Vector3 postion;

        GameObject[] CollectionforCaplsule = new GameObject[5];
        
        for (int i=0; i < maxPlatforms; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                postion = NextPosition(_displacement, true);
                if (!IsConstructionAllowed(postion))
                    postion = NextPosition(_displacement, false);
            }
            else
            {
                postion = NextPosition(_displacement, false);
                if (!IsConstructionAllowed(postion))
                    postion = NextPosition(_displacement, true);
            }
            
            _lastPlatformPosition = postion;
            platform = Instantiate(_currentPlatformPrefab, postion, Quaternion.identity);
            _platformsPool.Add(platform);

            CollectionforCaplsule[i % 5] = platform;
            
            if ((i + 1) % 5 == 0)
            {
                CreateCapsule(CollectionforCaplsule);
            }
        }
    }
    
    private Vector3 NextPosition(float displacement, bool onX)
    {
        Vector3 newPosition = _lastPlatformPosition;

        if (onX)
        {
            newPosition.x += displacement;
        }
        else
        {
            newPosition.z += displacement;
        }
        
        return newPosition;
    }
    
    private bool IsConstructionAllowed(Vector3 postion)
    {
        Collider[] hitColliders = Physics.OverlapBox(postion, _boxSizes, Quaternion.identity, _layerMaskForNextPlatform);

        return !(hitColliders.Length > 0);
    }
    
    private void CreateCapsule(GameObject[] gameObjects)
    {
        GameObject gameObjForCapsule = null;
        GameObject capsule = null;

        if(_capsuleRule == CapsuleRule.RandomFrom5)
        {
            gameObjForCapsule = gameObjects[Random.Range(0, 4)];
        }
        else
        {
            gameObjForCapsule = gameObjects[_n];
            _n++;
            if(_n > 4) _n = 0;
        }
        
        Vector3 pos = new Vector3(gameObjForCapsule.transform.position.x, 
                                                    _capsule.transform.position.y, 
                                                        gameObjForCapsule.transform.position.z);

        capsule = Instantiate(_capsule, pos, Quaternion.identity);
        capsule.transform.parent = gameObjForCapsule.transform;
    }
}