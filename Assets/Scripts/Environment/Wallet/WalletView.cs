using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;

    public void UpdateBalanceDisplay(float balance) =>
        _balanceText.text = balance.ToString("F0");
}