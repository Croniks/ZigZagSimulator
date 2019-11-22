﻿using UnityEngine;


public class LastPlatform : MonoBehaviour
{
    private GameManager _gameManager;
    private Transform _selfTransform;
    private float _distance = 20f;
    private Vector3 _position;
    private Vector3 _boxSizes;
    private LayerMask _layerMaskForCamera;


    void Start()
    {
        _gameManager = GameManager.Instance;
        _selfTransform = GetComponent<Transform>();

        _position = new Vector3(_selfTransform.position.x - _distance,
                                                            _selfTransform.position.y,
                                                            _selfTransform.position.z - _distance);

        _boxSizes = new Vector3(_selfTransform.localScale.x / 2,
                                    _selfTransform.localScale.y / 2,
                                        _selfTransform.localScale.z / 2);

        _layerMaskForCamera = _gameManager.layerMaskForCamera;
    }
    
    void Update()
    {
        if(IsCameraMovingCloser())
        {
            BuildPlatforms();
            Destroy(this);
        }
    }

    public void BuildPlatforms()
    {
        _gameManager.BuildPlatforms();
    }

    private bool IsCameraMovingCloser()
    {
        Collider[] hitColliders = Physics.OverlapBox(_position, _boxSizes, Quaternion.identity, _layerMaskForCamera);

        return hitColliders.Length > 0;
    }
}