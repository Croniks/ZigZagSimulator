using UnityEngine;
using UnityEngine.EventSystems;


public class UITransition : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UIPanel _current;
    [SerializeField] private UIPanel _next;
    
    public void OnPointerClick(PointerEventData data)
    {
        if(_current != null) { _current.IsOpened = false; }
        if(_next != null) { _next.IsOpened = true; }
    }
}