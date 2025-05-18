using UnityEngine;
using UnityEngine.UI;

public class SpeedGameRegulator : MonoBehaviour
{
    private const float MinimumSpeed = 0.01f;
    private const float MaximumSpeed = 5f;

    [SerializeField] private Slider _gameSpeed;

    private void Start() =>
        _gameSpeed.onValueChanged.AddListener(SetGameSpeed);

    private void OnDestroy() =>
        _gameSpeed.onValueChanged.RemoveListener(SetGameSpeed);

    private void SetGameSpeed(float speed) =>
        Time.timeScale = Mathf.Clamp(speed, MinimumSpeed, MaximumSpeed);
}