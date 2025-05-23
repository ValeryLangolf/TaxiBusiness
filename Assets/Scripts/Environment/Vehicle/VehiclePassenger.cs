using System;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePassenger
{
    private readonly Transform _transform;
    private readonly Action _passengerRefused;
    private Passenger _passenger;
    private bool _isInCar;

    public VehiclePassenger(Transform vehicle, Action passengerRefused)
    {
        _transform = vehicle;
        _passengerRefused = passengerRefused;
    }

    public Transform Transform => _transform;

    public bool IsInCar => _isInCar;

    public bool IsAssigned => _passenger != null;

    public Passenger Passenger => _passenger;

    public Vector3 Destination => _passenger.DestinationPoint.Position;

    public void AssignPassenger(Passenger passenger) =>
        SubscribePassanger(passenger);

    public void PutInCar()
    {
        _isInCar = true;
        _passenger.PickUp(this);
    }

    public void DropPassenger()
    {
        _isInCar = false;
        _passenger.ReturnInPool();
        UnsubscribePassanger(_passenger);
    }

    private void SubscribePassanger(Passenger passenger)
    {
        if (passenger == _passenger)
            return;

        UnsubscribePassanger(_passenger);

        if(passenger != null)
            passenger.AcceptOrder(this);
        
        _passenger = passenger;
    }

    public void UnsubscribePassanger(Passenger passenger)
    {
        if (passenger == null)
            return;

        passenger.CancelOrder(this);
        _passenger = null;
    }

    public void Refuse(Passenger passenger)
    {
        UnsubscribePassanger(passenger);
        _passengerRefused?.Invoke();
    }

    public float GetProfit(float moneyRate)
    {
        List<Waypoint> path = Pathfinder.FindPath(_passenger.DeparturePoint, _passenger.DestinationPoint);
        float distance = Utils.CalculateDistancePath(path);
        float profit = moneyRate * Constants.RatingMultiplier * distance;

        return profit;
    }
}