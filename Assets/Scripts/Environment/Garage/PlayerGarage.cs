using System.Collections.Generic;
using UnityEngine;

public class PlayerGarage : MonoBehaviour
{
    [SerializeField] private VehicleSelector _selector;
    [SerializeField] private VehicleSpawner _spawner;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private VehicleCard _prefabCard;
    [SerializeField] private CardParent _cardParent;

    private readonly List<VehicleParams> _vehicles = new();

    private void OnEnable()
    {
        _spawner.Spawned += OnSpawn;
        _selector.Selected += OnSelected;
        _selector.Deselected += OnDeselected;
    }

    private void OnDisable()
    {
        _spawner.Spawned -= OnSpawn;
        _selector.Selected -= OnSelected;
        _selector.Deselected -= OnDeselected;
    }

    private void OnSpawn(Vehicle vehicle)
    {
        VehicleCard card = Instantiate(_prefabCard, _cardParent.transform);
        VehicleParams vehicleParams = new(vehicle, card);
        _vehicles.Add(new(vehicle, card));
        SubscribeVehicle(vehicleParams);
    }

    private void OnSelected(Vehicle vehicle)
    {
        if (TryGetCard(vehicle, out VehicleCard card, _vehicles))
            card.Select();
    }

    private void OnDeselected(Vehicle vehicle)
    {
        if (TryGetCard(vehicle, out VehicleCard card, _vehicles))
            card.Deselect();
    }

    private void SubscribeVehicle(VehicleParams vehicleParams)
    {
        vehicleParams.Vehicle.PassengerDelivered += OnPassengerDelivered;
        vehicleParams.Card.Clicked += OnCardClicked;
    }

    private void OnPassengerDelivered(Vehicle vehicle)
    {
        int revenue = Random.Range(10, 500);
        _wallet.AddMoney(revenue);
    }

    private void OnCardClicked(VehicleCard vehicleCard)
    {
        if(TryGetVehicle(vehicleCard, out Vehicle vehicle, _vehicles))
            _selector.Select(vehicle);
    }

    private bool TryGetCard(Vehicle vehicle, out VehicleCard card, List<VehicleParams> vehicles)
    {
        card = null;

        foreach (VehicleParams vehicleParams in vehicles)
        {
            if (vehicleParams.Vehicle == vehicle)
            {
                card = vehicleParams.Card;

                return true;
            }
        }

        return false;
    }

    private bool TryGetVehicle(VehicleCard card, out Vehicle vehicle, List<VehicleParams> vehicles)
    {
        vehicle = null;

        foreach (VehicleParams vehicleParams in vehicles)
        {
            if (vehicleParams.Card == card)
            {
                vehicle = vehicleParams.Vehicle;

                return true;
            }
        }

        return false;
    }
}