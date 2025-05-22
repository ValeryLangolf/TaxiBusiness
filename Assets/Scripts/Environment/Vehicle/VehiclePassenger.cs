using System;
using UnityEngine;

public class VehiclePassenger
{
    private Passenger _passenger;
    private Transform _transform;

    private bool _isInCar;

    private readonly Action<Passenger> _refused;

    public VehiclePassenger(Transform vehicle, Action<Passenger> refused)
    {
        _transform = vehicle;
        _refused = refused;
    }

    public bool IsInCar => _isInCar;

    public bool IsAssigned => _passenger != null;

    public Passenger Passenger => _passenger;

    public Vector3 Destination => _passenger.Destination.Position;

    public void AssignPassenger(Passenger passenger)
    {
        if(passenger == null)
            throw new ArgumentNullException("Переданный в качестве аргумента пассажир не установлен");

        if (_passenger == passenger)
            return;

        SubscribePassanger(passenger);
        _passenger = passenger;
    }

    public void Reset() =>
        UnsubscribePassanger(_passenger);

    public void PutInCar()
    {
        _isInCar = true;
        _passenger.PickUp(_transform);
    }

    public void DropPassenger()
    {
        _isInCar = false;
        _passenger.Deselect();
        _passenger.ReturnInPool();
        UnsubscribePassanger(_passenger);
    }

    private void SubscribePassanger(Passenger passenger)
    {
        if (passenger == _passenger)
            return;

        UnsubscribePassanger(_passenger);

        passenger.Select();
        passenger.Refused += OnPassengerRefused;
        passenger.Taked += OnPassengerPickUp;
        _passenger = passenger;
    }

    private void UnsubscribePassanger(Passenger passenger)
    {
        if (_passenger == null)
            return;

        passenger.Deselect();
        passenger.Refused -= OnPassengerRefused;
        passenger.Taked -= OnPassengerPickUp;
        _passenger = null;
    }

    private void OnPassengerRefused(Passenger passenger)
    {
        UnsubscribePassanger(passenger);
        _refused?.Invoke(passenger);
    }

    private void OnPassengerPickUp(Passenger passenger)
    {
        if (_isInCar)
            return;

        UnsubscribePassanger(passenger);
        _refused?.Invoke(passenger);
    }
}