using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rb;
    private float _fallTimer;
    private bool _falling = false;
    

    void Start()
    {
        _fallTimer = GameManager.Instance.fallTimer;
    }

    void Update()
    {
        if(_falling)
        {
            _fallTimer -= Time.deltaTime;

            if(_fallTimer <= 0)
                PlatformCrashes();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);   
    }
    
    void OnCollisionExit(Collision other)
    {
        _falling = true;
    }

    private void PlatformCrashes()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
    }
}
