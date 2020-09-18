using System;
using System.Collections.Generic;


namespace Events
{
    public class CapsuleRuleChangedEvent
    {
        private readonly List<Action<int>> _callbacks = new List<Action<int>>();

        public void Subscribe(Action<int> callback)
        {
            _callbacks.Add(callback);
        }

        public void Publish(int capsulRule)
        {
            foreach (Action<int> callback in _callbacks)
                callback(capsulRule);
        }
    }
}