using UnityEngine;
using UnityEngine.EventSystems;


public class UITransition : MonoBehaviour, IPointerClickHandler
{
    private UIPanel _current;
    
    [SerializeField]
    private UIPanel _next;


    private void Awake()
    {
        _current = GetComponentInParent<UIPanel>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        _current.IsOpened = false;
        _next.IsOpened = true;
    }
}