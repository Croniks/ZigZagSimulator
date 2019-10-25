using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum LevelDifficulty { Easy, Normal, Hard }
public enum CapsuleRule { RandomFrom5, inOrder }

struct Displacement
{
    public float X { get; private set; }
    public float Z { get; private set; }

    public Displacement(float x, float z)
    {
        X = x;
        Z = z;
    }
}

public class GameManager : MonoBehaviour
{
    public CapsuleRule capsuleRule = CapsuleRule.inOrder;
    public LevelDifficulty levelDifficulty = LevelDifficulty.Hard;
    public LayerMask layerMaskForCamera;
    
    [SerializeField]
    private Ball _ball;
    [SerializeField]
    private LayerMask _layerMaskForNextPlatform;
    [SerializeField]
    private GameObject _platformEasy;
    [SerializeField]
    private GameObject _platformNormal;
    [SerializeField]
    private GameObject _platformHard;
    [SerializeField]
    private GameObject _capsule;
    [SerializeField]
    private Transform _startPlatform;
    [SerializeField]
    private int _maxNumberPlatforms = 100;
    [SerializeField]
    private RectTransform _endGamePanel;

    [SerializeField]
    private Text _pointsUI;
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

    private Vector3 _lastPlatformPosition;
    private Vector3 _boxSizes;
    private int _n = 0; 

    private Displacement _initialDisplacementOnX;
    private Displacement _initialDisplacementOnZ;
    private Displacement _displacementOnX;
    private Displacement _displacementOnZ;
    private GameObject _platformPrefab;


    void Start()
    {
        DefineLevelDifficulty(levelDifficulty);

        _lastPlatformPosition = _startPlatform.position;
        _boxSizes = new Vector3(_platformPrefab.transform.localScale.x / 2,
                                   _platformPrefab.transform.localScale.y / 2,
                                       _platformPrefab.transform.localScale.z / 2);

        InitializeLevel();
        BuildPlatforms();
    }
    
    public void StartGame()
    {
        _ball.StartMoving();
    }

    public void EndGame()
    {
        _ball.EndMoving();
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
        if(levelDifficulty == LevelDifficulty.Easy)
        {
            _initialDisplacementOnX = _displacementOnX = new Displacement(3f, 0f);
            _initialDisplacementOnZ = _displacementOnZ = new Displacement(0f, 3f);
            _platformPrefab = _platformEasy;
        }
        else if(levelDifficulty == LevelDifficulty.Normal)
        {
            _initialDisplacementOnX = new Displacement(2.5f, 0.5f);
            _initialDisplacementOnZ = new Displacement(0.5f, 2.5f);
            _displacementOnX = new Displacement(2f, 0f);
            _displacementOnZ = new Displacement(0f, 2f);
            _platformPrefab = _platformNormal;
        }
        else
        {
            _initialDisplacementOnX = new Displacement(2f, 1f);
            _initialDisplacementOnZ = new Displacement(1f, 2f);
            _displacementOnX = new Displacement(1f, 0f);
            _displacementOnZ = new Displacement(0f, 1f);
            _platformPrefab = _platformHard;
        }
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
                postion = NextPosition(_displacementOnX);
                if (!IsConstructionAllowed(postion))
                    postion = NextPosition(_displacementOnZ);
            }
            else
            {
                postion = NextPosition(_displacementOnZ);
                if (!IsConstructionAllowed(postion))
                    postion = NextPosition(_displacementOnX);
            }
            
            lastPlatform = Instantiate(_platformPrefab, postion, Quaternion.identity);
            _lastPlatformPosition = postion;
            CollectionforCaplsule[i % 5] = lastPlatform;
            
            if ((i + 1) % 5 == 0)
            { 
                CreateCapsule(CollectionforCaplsule);
            }
        }
        
        lastPlatform?.AddComponent<LastPlatform>();
    }
    
    private void InitializeLevel()
    {
        Vector3 postion;
        
        if (Random.Range(0, 2) == 0)
        {
            postion = NextPosition(_initialDisplacementOnX);
        }
        else
        {
            postion = NextPosition(_initialDisplacementOnZ);
        }

        Instantiate(_platformPrefab, postion, Quaternion.identity);
        _lastPlatformPosition = postion;
    }

    private Vector3 NextPosition(Displacement dis)
    {
        return new Vector3(_lastPlatformPosition.x + dis.X,
                                    _lastPlatformPosition.y,
                                        _lastPlatformPosition.z + dis.Z);
    }
    
    private bool IsConstructionAllowed(Vector3 postion)
    {
        Collider[] hitColliders = Physics.OverlapBox(postion, _boxSizes, Quaternion.identity, _layerMaskForNextPlatform);

        foreach (Collider col in hitColliders)
        {
            return false;
        }

        return true;
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