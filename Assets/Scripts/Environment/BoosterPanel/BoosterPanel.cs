using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BoosterType
{
    Passenger,
    Fuel,
    Repair,
}

public class BoosterPanel : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private GameObject _content;

    private List<BoosterCard> _cards;

    private float _passengerMultiplier = 1;

    public float PassengerMultiplier => _passengerMultiplier;

    private void Awake()
    {
        _cards = _content.GetComponentsInChildren<BoosterCard>(true).ToList();

        foreach (BoosterCard card in _cards)
            card.Init();
    }

    private void Update()
    {
        foreach (BoosterCard card in _cards)
        {
            card.UpdateTime();

            if (card.IsRepeat && card.IsOn == false)
                if (card.Type == BoosterType.Passenger)
                    if (_wallet.TrySpendMoney(card.Price))
                        card.Enable();
        }
    }

    private void OnEnable()
    {
        _wallet.ValueChanged += OnWalletValueChanged;

        foreach (BoosterCard card in _cards)
        {
            card.Clicked += OnCardClicked;
            card.Started += OnCardStarted;
            card.Completed += OnCardCompleted;
        }
    }

    private void OnDisable()
    {
        _wallet.ValueChanged -= OnWalletValueChanged;

        foreach (BoosterCard card in _cards)
        {
            card.Clicked -= OnCardClicked;
            card.Started -= OnCardStarted;
            card.Completed -= OnCardCompleted;
        }
    }

    private void OnWalletValueChanged(float balance)
    {
        foreach (BoosterCard card in _cards)
            card.SetInteractButton(balance >= card.Price);
    }

    private void OnCardStarted(BoosterCard card)
    {
        card.SetInteractButton(_wallet.Balance >= card.Price);

        if (card.Type == BoosterType.Passenger)
            RecalculatePassengerPercent();
        else if (card.Type == BoosterType.Fuel)
            FuelParams.SetCurrentPrice(card.Price);
        else if (card.Type == BoosterType.Repair)
            RepairParams.SetCurrentPrice(card.Price);
    }

    private void OnCardCompleted(BoosterCard card)
    {
        card.SetInteractButton(_wallet.Balance >= card.Price);

        if (card.Type == BoosterType.Passenger)
            RecalculatePassengerPercent();
        else if (card.Type == BoosterType.Fuel)
            FuelParams.SetDefaultPrice();
        else if (card.Type == BoosterType.Repair)
            RepairParams.SetDefaultPrice();
    }

    private void OnCardClicked(BoosterCard card)
    {
        if (_wallet.TrySpendMoney(card.Price) == false)
            return;

        card.Enable();
        SfxPlayer.Instance.PlayVehiclePurchased();
    }

    private void RecalculatePassengerPercent()
    {
        float passengerPercent = 0;

        foreach (BoosterCard card in _cards)
            if (card.Type == BoosterType.Passenger && card.IsOn)
                passengerPercent += card.PassengerPercent;

        _passengerMultiplier = 1f / (1f + passengerPercent / 100f);
    }
}