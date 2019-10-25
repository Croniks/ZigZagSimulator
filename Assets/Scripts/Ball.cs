using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    public float ballVelocity = 5;

    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Transform _cameraTransform;
    private Transform _selfTransform;
    private float _x, _y, _z = 0f;
    
    
    void Start()
    {
        _selfTransform = GetComponent<Transform>();
        _cameraTransform = _cameraTransform.GetComponent<Transform>();
    }
    
    public void StartMoving()
    {
        StartCoroutine(DelayBeforeStarting());
    }

    public void EndMoving()
    {
        StopAllCoroutines();
    }

    IEnumerator DelayBeforeStarting()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveBallOnZ());
        yield break;
    }
    
    IEnumerator MoveBallOnZ()
    {
        while (true)
        {
            _z = Time.deltaTime * ballVelocity;
            _selfTransform.Translate(_x, _y, _z, Space.World);
            _cameraTransform.Translate(_z / 2, _y, _z / 2, Space.World);

            if (Input.GetMouseButtonUp(0))
            {
                yield return new WaitForSeconds(0.01f);
                _z = 0f;

                StartCoroutine(MoveBallOnX());
                yield break;
            }
            
            yield return null;
        }
    }

    IEnumerator MoveBallOnX()
    {
        while (true)
        {
            _x = Time.deltaTime * ballVelocity;
            _selfTransform.Translate(_x, _y, _z, Space.World);
            _cameraTransform.Translate(_x / 2, _y, _x / 2, Space.World);

            if (Input.GetMouseButtonUp(0))
            {
                yield return new WaitForSeconds(0.01f);
                _x = 0f;

                StartCoroutine(MoveBallOnZ());
                yield break;
            }
            
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Border" || other.tag == "Fall")
        {
            _gameManager.EndGame();
        }
        else if(other.tag == "FallPlatform")
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Platform")
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}