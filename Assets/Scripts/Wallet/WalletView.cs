using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;

    private void Start()
    {
        Wallet.BalanceChanged += HandleBalanceChanged;
        UpdateBalanceDisplay(Wallet.Balance);
    }

    private void OnDestroy() =>
        Wallet.BalanceChanged -= HandleBalanceChanged;

    private void HandleBalanceChanged(int newBalance) =>
        UpdateBalanceDisplay(newBalance);

    private void UpdateBalanceDisplay(int balance) =>
        _balanceText.text = $"Баланс: {balance}р.";
}