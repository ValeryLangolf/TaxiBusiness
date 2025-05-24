using System;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private WalletView _view;
    [SerializeField] private int _initialBalance;

    public event Action<float> ValueChanged;

    private float _balance;

    public float Balance => _balance;

    private void Start()
    {
        _balance = Saver.LoadBalance();
        ProcessChange();
    }

    public void AddMoney(float amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Значение должно быть положительным!");

        _balance += amount;
        ProcessChange();
        SfxPlayer.Instance.PlayGettingRevenue();
    }

    public bool TrySpendMoney(float amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Значение должно быть положительным!");

        if (_balance < amount)
        {
            SfxPlayer.Instance.PlayUnsuccessfulPaymentAttempt();
            return false;
        }

        _balance -= amount;
        ProcessChange();

        return true;
    }

    public void EmptyWallet(float amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Значение должно быть положительным!");

        _balance -= amount;
        ProcessChange();        
    }

    private void ProcessChange()
    {
        _balance = Mathf.Max(_balance, 0);
        _view.UpdateBalanceDisplay(_balance);
        ValueChanged?.Invoke(_balance);
    }

    private void SaveGameData() =>
        Saver.SaveBalance(_balance);

    private void OnApplicationQuit() =>
        SaveGameData();
}