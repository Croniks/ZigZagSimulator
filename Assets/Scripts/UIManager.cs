using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _moveSpeed;
    [SerializeField] private Text _score;
    [SerializeField] private UIPanel _startPanel;
    [SerializeField] private RectTransform _backgroundMainPanel;


    public void SetMoveSpeedToUI(float speedMin, float speedMax, float speed)
    {
        _moveSpeed.value = (((speed - speedMin) / (speedMax - speedMin)) * 100);
    }
    
    public float GetMoveSpeedFromUI(float speedMin, float speedMax)
    {
        return speedMin + ((_moveSpeed.value * (speedMax - speedMin)) / 100);
    }

    public void ChangeScore(int number)
    {
        _score.text = number.ToString();
    }
}