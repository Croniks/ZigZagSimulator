using UnityEngine;


namespace Events
{
    public class CRChangeEventInitiator : MonoBehaviour
    {
        public void InitiateEvent(int capsuleRule)
        {
            EventAggregator.CapsuleRuleChangedEvent?.Publish(capsuleRule);
        }
    }
}