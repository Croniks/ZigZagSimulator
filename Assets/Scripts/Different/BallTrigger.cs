using UnityEngine;


public class BallTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Rigidbody>().Sleep();
        other.gameObject.SetActive(false);
    }
}
