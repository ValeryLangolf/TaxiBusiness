using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DispatcherCenter : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private PlayerGarage _garage;
    [SerializeField] private PassengerSpawner _passengerSpawner;
    [SerializeField] private Button _buttonAdd;
    [SerializeField] private DispatcherCard _prefab;
    [SerializeField] private Transform _content;

    private readonly List<DispatcherCard> _cards = new();

    private void OnEnable() =>
        _buttonAdd.onClick.AddListener(OnClickAdd);

    private void OnDisable() =>
        _buttonAdd.onClick.RemoveListener(OnClickAdd);

    private void OnClickAdd()
    {
        DispatcherCard card = Instantiate(_prefab, _content);
        _cards.Add(card);
        Subscribe(card);
    }

    private void Subscribe(DispatcherCard card) =>
        card.CycleCompleted += OnCardCycleCompleted;

    private void OnCardCycleCompleted(DispatcherCard card)
    {
        _wallet.EmptyWallet(card.SalaryRate);
        AssignPassengerVehicle();
    }

    private void AssignPassengerVehicle()
    {
        if (TryGetRandomVehicle(out Vehicle vehicle) == false)
            return;

        if (TryGetRandomPassenger(out Passenger passenger) == false) 
            return;

        vehicle.SetPassenger(passenger);
        vehicle.SetDestination(passenger.Target.position);
    }

    private bool TryGetRandomVehicle(out Vehicle vehicle)
    {
        vehicle = null;
        List<VehicleParams> vehicles = _garage.GetAvailable();

        if (vehicles.Count == 0)
            return false;

        int randomId = Random.Range(0, vehicles.Count);
        vehicle = vehicles[randomId].Vehicle;

        return true;
    }

    private bool TryGetRandomPassenger(out Passenger passenger)
    {
        passenger = null;

        List<Passenger> passengers = _passengerSpawner.GetAwailable();

        if(passengers.Count == 0)
            return false;

        int randomId = Random.Range(0, passengers.Count);
        passenger = passengers[randomId];

        return true;
    }
}