using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGarage : MonoBehaviour
{
    [SerializeField] private Vehicle _initialPrefab;
    [SerializeField] private Transform _initialTransform;
    [SerializeField] private Shop _shop;
    [SerializeField] private VehicleSelector _selector;
    [SerializeField] private VehicleSpawner _spawner;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private VehicleIcon _iconPrefab;
    [SerializeField] private IconContent _iconContent;
    [SerializeField] private VehicleWorldIndicatorSpawner _lackIndicatorSpawner;
    [SerializeField] private DriverManSpawner _driverManSpawner;

    private readonly List<VehicleIcon> _cards = new();

    public event Action<float, Vector3> MoneyAdded;
    public event Action<Vehicle> Added;
    public event Action<Vehicle> WillBeRemoved;

    public List<VehicleIcon> Cards => new(_cards);

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

    public void Init(List<Saver.VehicleSaveData> vehicleSaveData)
    {
        foreach (VehicleIcon vehicleParams in new List<VehicleIcon>(_cards))
            RemoveVehicle(vehicleParams.Vehicle);

        ClearAllChildren(_iconContent.transform);

        if (vehicleSaveData.Count == 0)
        {
            _spawner.Spawn(_initialPrefab, _initialTransform.position, _initialTransform.rotation);
            return;
        }

        foreach (Saver.VehicleSaveData vehicleData in vehicleSaveData)
        {
            Vehicle prefab = _shop.GetVehiclePrefab(vehicleData.Name);
            Vehicle vehicle = _spawner.Spawn(prefab, vehicleData.Position, vehicleData.Rotation);
            vehicle.Params.SetRemainingFuel(vehicleData.RemainingFuel);
            vehicle.Params.SetRemainingRepair(vehicleData.RemainingRepair);

            if (vehicleData.IsDriverMan)
                vehicle.SetDriverMan(_driverManSpawner.Spawn());
        }
    }

    public List<VehicleIcon> GetAvailable() =>
        _cards.Where(v => v.Vehicle.IsActivePath == false 
        && v.Vehicle.IsPassengerAssigned == false
        && v.Vehicle.Params.CanGo).ToList();

    public void RemoveVehicle(Vehicle vehicle)
    {
        if (TryGetCard(vehicle, out VehicleIcon card, _cards) == false)
            return;

        WillBeRemoved?.Invoke(vehicle);

        _cards.RemoveAll(v => v.Vehicle == vehicle);
        UnsubscribeVehicle(card);
        Destroy(card.gameObject);
        Destroy(vehicle.gameObject);
    }

    private void ClearAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
            Destroy(child.gameObject);
    }

    private void OnSpawn(Vehicle vehicle)
    {
        VehicleIcon card = Instantiate(_iconPrefab, _iconContent.transform);        
        card.Init(vehicle);
        _cards.Add(card);
        SubscribeVehicle(card);
        Added?.Invoke(vehicle);
    }

    private void OnSelected(Vehicle vehicle)
    {
        if (TryGetCard(vehicle, out VehicleIcon card, _cards) == false)
            return;

        card.Select();
    }

    private void OnDeselected(Vehicle vehicle)
    {
        if (TryGetCard(vehicle, out VehicleIcon card, _cards))
            card.Deselect();
    }

    private void SubscribeVehicle(VehicleIcon card)
    {
        card.Vehicle.PassengerDelivered += OnPassengerDelivered;
        card.Vehicle.MoneySpended += OnMoneySpended;
        card.Vehicle.Params.FuelWasted += OnFuelWasted;
        card.Vehicle.Params.RepairWasted += OnRepairWasted;
        card.Clicked += OnCardClicked;
    }

    private void UnsubscribeVehicle(VehicleIcon card)
    {
        card.Vehicle.PassengerDelivered -= OnPassengerDelivered;
        card.Vehicle.MoneySpended -= OnMoneySpended;
        card.Vehicle.Params.FuelWasted -= OnFuelWasted;
        card.Vehicle.Params.RepairWasted -= OnRepairWasted;
        card.Clicked -= OnCardClicked;
    }

    private void OnPassengerDelivered(Vehicle vehicle, float profit)
    {
        _wallet.AddMoney(profit);
        MoneyAdded?.Invoke(profit, vehicle.Position);
    }

    private void OnMoneySpended(float value) =>
        _wallet.SpendOnEmptyWallet(value);

    private void OnCardClicked(VehicleIcon vehicleCard)
    {
        if (TryGetVehicle(vehicleCard, out Vehicle vehicle, _cards))
            _selector.Select(vehicle);
    }

    private bool TryGetCard(Vehicle vehicle, out VehicleIcon vehicleIcon, List<VehicleIcon> cards)
    {
        vehicleIcon = null;

        foreach (VehicleIcon card in cards)
        {
            if (card.Vehicle == vehicle)
            {
                vehicleIcon = card;
                return true;
            }
        }

        return false;
    }

    private bool TryGetVehicle(VehicleIcon vehicleIcon, out Vehicle vehicle, List<VehicleIcon> cards)
    {
        vehicle = null;

        foreach (VehicleIcon card in cards)
        {
            if (vehicleIcon == card)
            {
                vehicle = card.Vehicle;
                return true;
            }
        }

        return false;
    }

    private void OnFuelWasted(VehicleParams vehicleParams) =>
        _lackIndicatorSpawner.SpawnFuelIndicator(vehicleParams);
    
    private void OnRepairWasted(VehicleParams vehicleParams) =>
        _lackIndicatorSpawner.SpawnRepairIndicator(vehicleParams);
}