using System;
using System.Collections.Generic;


namespace Events
{
    public class BallVelocityChangedEvent
    {
        private readonly List<Action<float>> _callbacks = new List<Action<float>>();

        public void Subscribe(Action<float> callback)
        {
            _callbacks.Add(callback);
        }

        public void Publish(float moveSpeed)
        {
            foreach (Action<float> callback in _callbacks)
                callback(moveSpeed);
        }
    }
}