using System;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private Mover _mover;
    private Rotator _rotator;
    private VehiclePathKeeper _pathKeeper;
    private VehiclePassenger _vehiclePassenger;

    private Sprite _sprite;
    private float _moneyRate;
    private float _strength;
    private float _petrol;
    private float _price;

    private List<Waypoint> _points;

    public event Action<Vehicle> PathDestinated;
    public event Action<Vehicle> PathCompleted;
    public event Action<Vehicle, float> PassengerDelivered;

    public bool IsActivePath => _pathKeeper.IsActivePath;

    public Waypoint EndPoint => _pathKeeper.EndPoint;

    public List<Waypoint> RemainingPath => new(_pathKeeper.RemainingPath);

    public bool IsPassengerInCar => _vehiclePassenger.IsInCar;

    public bool IsPassengerAssigned => _vehiclePassenger.IsAssigned;

    public Vector3 Position => transform.position;

    public Sprite Sprite => _sprite;

    private void Awake()
    {
        _mover = new(transform);
        _rotator = new(transform);
        _pathKeeper = new(transform, OnPathCompleted);
        _vehiclePassenger = new(transform, OnPassengerRefused);
    }

    private void Start() =>
        _points = RoadNetwork.Instance.Points;

    private void Update()
    {
        if (_pathKeeper.IsActivePath == false)
            return;

        _mover.Move(_pathKeeper.CurrentTarget);
        _pathKeeper.UpdatePath();

        if (_pathKeeper.IsActivePath)
            _rotator.Rotate(_pathKeeper.CurrentTarget);
    }

    public void InitParams(VehicleConfig vehicleSO)
    {
        _mover.UpdateSpeed(vehicleSO.Speed);

        _moneyRate = vehicleSO.MoneyRate;
        _sprite = vehicleSO.CarImage;
        _strength = vehicleSO.Strength;
        _petrol = vehicleSO.Petrol;
        _price = vehicleSO.Price;
    }

    public void AssignPassenger(Passenger passenger) =>
        _vehiclePassenger.AssignPassenger(passenger);

    public void SetDestination(Vector3 destination)
    {
        if (_points == null || _points.Count == 0) 
            return;

        Waypoint start = Utils.GetNearestSectionAndPoint(transform.position, _points);
        Waypoint end = Utils.GetNearestSectionAndPoint(destination, _points);

        if (start == null || end == null || start == end) 
            return;

        List<Waypoint> path = Pathfinder.FindPath(start, end);
        _pathKeeper.SetPath(path ?? new List<Waypoint>());

        if (path?.Count > 0)
            PathDestinated?.Invoke(this);
    }

    private void OnPathCompleted()
    {
        PathCompleted?.Invoke(this);

        if (_vehiclePassenger.IsInCar)
        {
            float profit = GetProfit(_vehiclePassenger.Passenger);
            _vehiclePassenger.DropPassenger();
            PassengerDelivered?.Invoke(this, profit);
        }
        else if (_vehiclePassenger.Passenger != null)
        {
            _vehiclePassenger.PutInCar();
            SetDestination(_vehiclePassenger.Destination);
        }
    }

    private void OnPassengerRefused(Passenger passenger)
    {
        _pathKeeper.ResetPath();
        PathCompleted?.Invoke(this);
    }

    private float GetProfit(Passenger passenger)
    {
        List<Waypoint> path = Pathfinder.FindPath(passenger.Departure, passenger.Destination);
        float distance = Utils.CalculateDistancePath(path);
        float profit = _moneyRate * distance;

        return profit;
    }
}