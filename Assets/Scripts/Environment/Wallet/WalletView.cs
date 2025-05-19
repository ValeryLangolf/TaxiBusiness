using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    private TextMeshProUGUI _balanceText;

    private void Awake() =>
        _balanceText = GetComponent<TextMeshProUGUI>();

    public void UpdateBalanceDisplay(float balance) =>
        _balanceText.text = balance.ToString("F0");
}