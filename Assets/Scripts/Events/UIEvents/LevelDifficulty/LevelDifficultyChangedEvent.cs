using System;
using System.Collections.Generic;


namespace Events
{
    public class LevelDifficultyChangedEvent
    {
        private readonly List<Action<int>> _callbacks = new List<Action<int>>();

        public void Subscribe(Action<int> callback)
        {
            _callbacks.Add(callback);
        }

        public void Unsubscribe(Action<int> callback)
        {
            _callbacks.Remove(callback);
        }
        
        public void Publish(int levelDifficulty)
        {
            foreach (Action<int> callback in _callbacks)
                callback(levelDifficulty);
        }
    }
}