using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private VehicleSpawner _spawner;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private List<Vehicle> _vehicles;
    [SerializeField] private VehicleShopCard _prefab;
    [SerializeField] private Transform _content;

    private readonly Dictionary<VehicleShopCard, Vehicle> _cards = new();

    private void Awake()
    {
        if (_prefab == null)
            throw new NullReferenceException("������ �� ����������");

        foreach (Vehicle vehicle in _vehicles)
        {
            VehicleShopCard cardView = Instantiate(_prefab, _content);
            _cards.Add(cardView, vehicle);
            cardView.Initialize(vehicle, OnCardClicked);
        }

        OnWalletValueChanged(_wallet.Balance);
    }

    private void OnEnable() =>
        _wallet.ValueChanged += OnWalletValueChanged;

    private void OnDisable() =>
        _wallet.ValueChanged -= OnWalletValueChanged;

    public Vehicle GetVehiclePrefab(string name)
    {
        foreach (Vehicle vehicle in _vehicles)
            if (vehicle.Name == name)
                return vehicle;

        return null;
    }

    private void OnCardClicked(VehicleShopCard card)
    {
        if (_cards.TryGetValue(card, out Vehicle vehiclePrefab) == false)
            return;

        if (_wallet.TrySpendMoney(vehiclePrefab.Price) == false)
            return;

        _spawner.Spawn(vehiclePrefab);
        SfxPlayer.Instance.PlayVehiclePurchased();
    }

    private void OnWalletValueChanged(float balance)
    {
        foreach (VehicleShopCard card in _cards.Keys)
        {
            if (_cards.TryGetValue(card, out Vehicle vehiclePrefab))
                card.SetInteractButton(balance >= vehiclePrefab.Price);
        }
    }
}