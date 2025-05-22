using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private VehicleSpawner _spawner;
    [SerializeField] private List<VehicleConfig> _vehicles;
    [SerializeField] private VehicleShopCard _prefab;
    [SerializeField] private Transform _content;

    private Dictionary<VehicleShopCard, VehicleConfig> _cards = new();

    private void Awake()
    {
        if (_prefab == null)
            throw new NullReferenceException("Префаб не установлен");

        foreach (VehicleConfig vehicleSO in _vehicles)
        {
            VehicleShopCard cardView = Instantiate(_prefab, _content);
            _cards.Add(cardView, vehicleSO);
            cardView.Initialize(vehicleSO, OnCardClicked);
        }
    }

    private void Start()
    {
        if (_vehicles.Count > 0)
            Spawn(_vehicles[0]);
    }

    private void OnCardClicked(VehicleShopCard card)
    {
        if (_cards.TryGetValue(card, out VehicleConfig vehicle))
            Spawn(vehicle);
        else
            Debug.LogError("Clicked card not found in dictionary!");
    }

    private void Spawn(VehicleConfig vehicleSO) =>
        _spawner.Spawn(vehicleSO);
}