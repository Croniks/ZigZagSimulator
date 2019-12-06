using UnityEngine;


public class UIPanel : MonoBehaviour
{
    private Animator _animator;
    
    
    public bool IsOpened
    {
        get { return _animator.GetBool("IsOpen"); }
        set
        {
            //if(!gameObject.activeInHierarchy) gameObject.SetActive(true);
            //if(value) transform.SetAsFirstSibling();
            _animator.SetBool("IsOpen", value);
        }
    }
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    void Enable()
    {
        if(_animator) return;
        _animator = GetComponent<Animator>();
    }
}