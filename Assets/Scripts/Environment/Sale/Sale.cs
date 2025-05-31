using System;
using System.Collections.Generic;
using UnityEngine;

public class Sale : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private PlayerGarage _garage;
    [SerializeField] private Transform _content;
    [SerializeField] private VehicleSaleCard _prefab;
    [SerializeField, Range(0, 1)] private float _salesIncome;

    private readonly Dictionary<VehicleSaleCard, Vehicle> _cards = new();

    private void Awake()
    {
        if (_prefab == null)
            throw new NullReferenceException("Префаб не установлен");
    }

    private void OnEnable()
    {
        _garage.Added += OnAddVehicle;
        _garage.WillBeRemoved += OnRemoveVehicle;
    }

    private void OnDisable()
    {
        _garage.Added -= OnAddVehicle;
        _garage.WillBeRemoved -= OnRemoveVehicle;
    }

    private void OnAddVehicle(Vehicle vehicle)
    {
        VehicleSaleCard cardView = Instantiate(_prefab, _content);
        _cards.Add(cardView, vehicle);
        cardView.Initialize(vehicle, _salesIncome, OcSaleClicked);
        UpdateButtonSelection();
    }

    private void OnRemoveVehicle(Vehicle vehicle)
    {
        VehicleSaleCard cardToRemove = null;

        foreach (var entry in _cards)
        {
            if (entry.Value != vehicle)
                continue;

            cardToRemove = entry.Key;
            break;
        }

        if (cardToRemove == null)
            return;

        Destroy(cardToRemove.gameObject);
        _cards.Remove(cardToRemove);
        UpdateButtonSelection();
    }

    private void OcSaleClicked(VehicleSaleCard card)
    {
        if (_cards.TryGetValue(card, out Vehicle vehicle) == false)
            return;

        _wallet.AddMoney(vehicle.Params.Price * _salesIncome);
        _garage.RemoveVehicle(vehicle);
    }

    private void UpdateButtonSelection()
    {
        if (_cards.Count == 1)
        {
            VehicleSaleCard firstCard = null;

            foreach (var card in _cards.Keys)
            {
                firstCard = card;
                break;
            }

            if (firstCard != null)
                firstCard.SetInteractButton(false);

            return;
        }

        foreach (VehicleSaleCard card in _cards.Keys)
        {
            if (_cards.TryGetValue(card, out Vehicle _))
                card.SetInteractButton(true);
        }
    }
}