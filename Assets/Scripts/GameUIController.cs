using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private Slider _gameSpeed;

    private void Start()
    {
        EconomyManager.OnBalanceChanged += HandleBalanceChanged;
        _gameSpeed.onValueChanged.AddListener(SetGameSpeed);

        // Первоначальное обновление
        UpdateBalanceDisplay(EconomyManager.Balance);
    }

    private void OnDestroy() =>
        EconomyManager.OnBalanceChanged -= HandleBalanceChanged;

    private void SetGameSpeed(float speed) =>
        Time.timeScale = Mathf.Clamp(speed, 0.1f, 5f);

    private void HandleBalanceChanged(int newBalance) =>
        UpdateBalanceDisplay(newBalance);

    private void UpdateBalanceDisplay(int balance) =>
        _balanceText.text = $"Баланс: {balance}$";
}