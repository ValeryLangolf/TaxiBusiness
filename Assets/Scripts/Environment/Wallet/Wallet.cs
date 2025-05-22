using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private WalletView _view;
    [SerializeField] private int _initialBalance;

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

        SfxPlayer.Instance.PlayGettingRevenue();
    }

    public bool TrySpendMoney(int amount)
    {
        if (_balance < amount)
            return false;

        _balance -= amount;
        UpdateView();

        return true;
    }

    private void UpdateView() =>
        _view.UpdateBalanceDisplay(_balance);
}