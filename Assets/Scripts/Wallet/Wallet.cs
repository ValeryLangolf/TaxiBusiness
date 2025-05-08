using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _initialBalance = 5000;

    private static int _balance;

    public static event Action<int> BalanceChanged;

    public static int Balance => _balance;

    private void Awake()
    {
        _balance = _initialBalance;
        UpdateBalanceDisplay();
    }

    public static void AddMoney(int amount)
    {
        _balance += amount;
        UpdateBalanceDisplay();
    }

    public static bool TrySpendMoney(int amount)
    {
        if (_balance < amount)
            return false;

        _balance -= amount;
        UpdateBalanceDisplay();

        return true;
    }

    private static void UpdateBalanceDisplay() =>
        BalanceChanged?.Invoke(_balance);
}