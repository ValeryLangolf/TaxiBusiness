using System;
using UnityEngine;

public class VehicleSeat
{
    private Passenger _passenger;
    private Transform _transform;

    private bool _isInCar;
    private readonly Action<Passenger> _refused;

    public VehicleSeat(Transform vehicle, Action<Passenger> refused)
    {
        _transform = vehicle;
        _refused = refused;
    }

    public bool IsInCar => _isInCar;

    public Passenger Passenger => _passenger;

    public Vector3 Destination => _passenger.Destination.Position;

    public void SetWaitingPassenger(Passenger passenger)
    {
        if (_passenger == passenger)
            return;

        if (_passenger != null)
            Reset();

        SubscribePassanger(passenger);
        _passenger = passenger;
    }

    public void Reset()
    {
        if (_passenger != null)
            UnsubscribePassanger(_passenger);

        _passenger = null;
    }

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
        Reset();
    }

    private void SubscribePassanger(Passenger passenger)
    {
        passenger.Select();
        passenger.Refused += OnPassengerRefused;
    }

    private void UnsubscribePassanger(Passenger passenger)
    {
        passenger.Deselect();
        passenger.Refused -= OnPassengerRefused;
    }

    private void OnPassengerRefused(Passenger passenger)
    {
        Reset();
        _refused?.Invoke(passenger);
    }
}