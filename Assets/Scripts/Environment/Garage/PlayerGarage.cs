using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGarage : MonoBehaviour
{
    [SerializeField] private VehicleSelector _selector;
    [SerializeField] private VehicleSpawner _spawner;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private VehicleIcon _iconPrefab;
    [SerializeField] private IconContect _iconContent;

    private readonly List<VehicleParams> _vehicles = new();

    public List<VehicleParams> Vehicles => new(_vehicles);

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

    public List<VehicleParams> GetAvailable() =>
        _vehicles.Where(v => v.Vehicle.IsActivePath == false && v.Vehicle.IsPassengerAssigned == false).ToList();

    private void OnSpawn(Vehicle vehicle)
    {
        VehicleIcon icon = Instantiate(_iconPrefab, _iconContent.transform);
        icon.SetIcon(vehicle.Sprite);

        VehicleParams vehicleParams = new(vehicle, icon);
        _vehicles.Add(new(vehicle, icon));
        SubscribeVehicle(vehicleParams);
    }

    private void OnSelected(Vehicle vehicle)
    {
        if (TryGetCard(vehicle, out VehicleIcon card, _vehicles) == false)
            return;

        card.Select();
    }

    private void OnDeselected(Vehicle vehicle)
    {
        if (TryGetCard(vehicle, out VehicleIcon card, _vehicles))
            card.Deselect();
    }

    private void SubscribeVehicle(VehicleParams vehicleParams)
    {
        vehicleParams.Vehicle.PassengerDelivered += OnPassengerDelivered;
        vehicleParams.Card.Clicked += OnCardClicked;
    }

    private void OnPassengerDelivered(Vehicle vehicle, float profit)
    {
        _wallet.AddMoney(profit);
        Debug.Log($"Доход составил: {profit}");
    }

    private void OnCardClicked(VehicleIcon vehicleCard)
    {
        if (TryGetVehicle(vehicleCard, out Vehicle vehicle, _vehicles))
            _selector.Select(vehicle);
    }

    private bool TryGetCard(Vehicle vehicle, out VehicleIcon card, List<VehicleParams> vehicles)
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

    private bool TryGetVehicle(VehicleIcon card, out Vehicle vehicle, List<VehicleParams> vehicles)
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