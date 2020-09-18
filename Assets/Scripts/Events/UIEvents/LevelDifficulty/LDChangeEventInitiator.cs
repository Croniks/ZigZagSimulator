using UnityEngine;


namespace Events
{
    public class LDChangeEventInitiator : MonoBehaviour
    {
        public void InitiateEvent(int levelDifficulty)
        {
            EventAggregator.LevelDifficultyChangedEvent?.Publish(levelDifficulty);
        }
    }
}