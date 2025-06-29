using System.Collections.Generic;
using UnityEngine;

public class AdvertisingPanel : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private List<AdvertisingCard> _cards;

    private float _passengerPercent;

    public float PassengerPercent => _passengerPercent;

    private void Awake()
    {
        foreach (AdvertisingCard card in _cards)
            card.Init();
    }

    private void Update()
    {
        foreach (AdvertisingCard card in _cards)
            card.UpdateTime();
    }

    private void OnEnable()
    {
        _wallet.ValueChanged += OnWalletValueChanged;

        foreach (AdvertisingCard card in _cards)
        {
            card.Started += OnCardStarted;
            card.Completed += OnCardCompleted;
            card.Clicked += OnCardClicked;
        }
    }

    private void OnDisable()
    {
        _wallet.ValueChanged -= OnWalletValueChanged;

        foreach (AdvertisingCard card in _cards)
        {
            card.Started -= OnCardStarted;
            card.Completed -= OnCardCompleted;
            card.Clicked -= OnCardClicked;
        }
    }

    private void OnWalletValueChanged(float balance)
    {
        foreach (AdvertisingCard card in _cards)
            card.SetInteractButton(balance >= card.Price);
    }

    private void OnCardStarted(AdvertisingCard _) =>
        RecalculatePassengerPercent();

    private void OnCardCompleted(AdvertisingCard _) =>
        RecalculatePassengerPercent();

    private void OnCardClicked(AdvertisingCard card)
    {
        if (_wallet.TrySpendMoney(card.Price))
            return;

        card.EnableCard();
        SfxPlayer.Instance.PlayVehiclePurchased();
    }

    private void RecalculatePassengerPercent()
    {
        _passengerPercent = 0;

        foreach (AdvertisingCard card in _cards)
            if (card.IsOn)
                _passengerPercent += card.PassengerPercent;

        Debug.Log($"_passengerPercent = {_passengerPercent}");
    }
}