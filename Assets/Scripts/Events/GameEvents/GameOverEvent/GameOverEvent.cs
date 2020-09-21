using System;
using System.Collections.Generic;


namespace Events
{
    public class GameOverEvent
    {
        private readonly List<Action> _callbacks = new List<Action>();

        public void Subscribe(Action callback)
        {
            _callbacks.Add(callback);
        }

        public void Unsubscribe(Action callback)
        {
            _callbacks.Remove(callback);
        }

        public void Publish()
        {
            foreach (Action callback in _callbacks)
                callback();
        }
    }
}