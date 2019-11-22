using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum LevelDifficulty { Easy, Normal, Hard }
public enum CapsuleRule { RandomFrom5, inOrder }


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelDifficulty levelDifficulty = LevelDifficulty.Hard;
    public CapsuleRule capsuleRule = CapsuleRule.inOrder;
    public float fallTimer = 2f;
    
    [SerializeField]
    private Ball _ball;
    [SerializeField]
    private GameObject[] _platformPrefabs;
    [SerializeField]
    private Transform[] _startPlatforms;
    [SerializeField]
    private GameObject _capsule;
    [SerializeField]
    private RectTransform _endGamePanel;
    [SerializeField]
    private Text _pointsUI;
    [SerializeField]
    private int _maxNumberPlatforms = 100;

    [SerializeField]
    public LayerMask layerMaskForCamera;
    [SerializeField]
    private LayerMask _layerMaskForNextPlatform;
    
    private GameObject _currentPlatformPrefab;
    private Transform _currentStartPlatform;

    private Vector3 _boxSizes;
    private float _displacement;
    private Vector3 _lastPlatformPosition;
    
    private int _numberOfPoints;
    public int NumberOfPoints
    {
        get
        {
            return _numberOfPoints;
        }
        set
        {
            _numberOfPoints += value;
            _pointsUI.text = _numberOfPoints.ToString();
        }
    }
    
    private int _n = 0; 

    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        DefineLevelDifficulty(levelDifficulty);
        BuildPlatforms();
    }

    public void StartGame()
    {
        _ball.StartMoving();
    }

    public void EndGame()
    {
        _endGamePanel.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
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

    public void BuildPlatforms()
    {
        GameObject lastPlatform = null;
        Vector3 postion;

        GameObject[] CollectionforCaplsule = new GameObject[5];
        
        for (int i=0; i < _maxNumberPlatforms; i++)
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
            
            lastPlatform = Instantiate(_currentPlatformPrefab, postion, Quaternion.identity);
            _lastPlatformPosition = postion;

            CollectionforCaplsule[i % 5] = lastPlatform;
            
            if ((i + 1) % 5 == 0)
            {
                CreateCapsule(CollectionforCaplsule);
            }
        }
        
        lastPlatform?.AddComponent<LastPlatform>();
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

        if(capsuleRule == CapsuleRule.RandomFrom5)
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

        Instantiate(_capsule, pos, Quaternion.identity);
    }
}