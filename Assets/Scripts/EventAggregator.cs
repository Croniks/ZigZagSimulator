using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EventAggregator : MonoBehaviour
{
    private static EventAggregator _eventAggregator;
    public static EventAggregator Instance
    {
        get
        {
            if(_eventAggregator == null)
                _eventAggregator = new EventAggregator();
            return _eventAggregator;
        }
    }
}