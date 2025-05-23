using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private WalletView _view;
    [SerializeField] private int _initialBalance;

    public event Action<float> ValueChanged;

    private float _balance;

    public float Balance => _balance;

    private void Awake() =>
        _balance = _initialBalance;

    private void Start() =>
        UpdateView();

    public void AddMoney(float amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Нельзя добавлять в кошелёк отрицательное количество денег!");
            return;
        }

        _balance += amount;
        UpdateView();

        ValueChanged?.Invoke(_balance);
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

        UpdateView();
        ValueChanged?.Invoke(_balance);

        return true;
    }

    public void EmptyWallet(float amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Значение должно быть положительным!");

        _balance -= amount;

        if(_balance < 0)
            _balance = 0;

        UpdateView();
        ValueChanged?.Invoke(_balance);
    }

    private void UpdateView() =>
        _view.UpdateBalanceDisplay(_balance);
}