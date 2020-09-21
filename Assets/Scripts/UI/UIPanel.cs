using UnityEngine;


[RequireComponent(typeof(Animator))]
public class UIPanel : MonoBehaviour
{
    private Animator _animator;
    
    public bool IsOpened
    {
        get { return _animator.GetBool("IsOpen"); }
        set
        {
            _animator.SetBool("IsOpen", value);
        }
    }
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}