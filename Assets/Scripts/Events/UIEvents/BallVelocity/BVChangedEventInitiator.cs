using UnityEngine;


namespace Events
{
    public class BVChangedEventInitiator : MonoBehaviour
    {
        public void InitiateEvent(float moveSpeed)
        {
            moveSpeed = UIManager.Instance.TransformUIValueToRealMoveSpeed(
                SettingsManager.Instance.GetMoveSpeedMin(),
                    SettingsManager.Instance.GetMoveSpeedMax(),
                                                    moveSpeed
            );

            EventAggregator.BallVelocityChangedEvent?.Publish(moveSpeed);
        }
    }
}