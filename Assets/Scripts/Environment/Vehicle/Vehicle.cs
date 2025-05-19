using System;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _moneyRate;

    private Mover _mover;
    private Rotator _rotator;
    private VehiclePathKeeper _pathKeeper;
    private VehiclePassenger _vehiclePassenger;

    private List<Waypoint> _points;

    public event Action<Vehicle> PathDestinated;
    public event Action<Vehicle> PathCompleted;
    public event Action<Vehicle> PassengerTransportationStarted;
    public event Action<Vehicle, float> PassengerDelivered;

    public bool IsActivePath => _pathKeeper.IsActivePath;

    public Waypoint EndPoint => _pathKeeper.EndPoint;

    public List<Waypoint> RemainingPath => new(_pathKeeper.RemainingPath);

    public bool IsPassengerInCar => _vehiclePassenger.IsInCar;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        _mover = new(transform, _speed);
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

    public void SetWaitingPassenger(Passenger passenger) =>
        _vehiclePassenger.SetWaitingPassenger(passenger);

    public void ResetPassenger() =>
        _vehiclePassenger.Reset();

    public void SetDestination(Vector3 destination)
    {
        Waypoint start = Utils.GetNearestSectionAndPoint(transform.position, _points);
        Waypoint end = Utils.GetNearestSectionAndPoint(destination, _points);

        if (start == end)
            return;

        List<Waypoint> path = Pathfinder.FindPath(start, end);

        if (path == null)
        {
            Debug.Log($"Твой путь Null");
            return;
        }

        if (path.Count == 0)
        {
            Debug.Log($"Для пути не найдено ни одной точки");
            return;
        }

        _pathKeeper.SetPath(path);
        PathDestinated?.Invoke(this);

        SfxPlayer.Instance.PlayEngineRoar();
    }

    private void OnPathCompleted()
    {
        if (_vehiclePassenger.IsInCar)
        {
            Passenger passenger = _vehiclePassenger.Passenger;
            List<Waypoint> path = Pathfinder.FindPath(passenger.Departure, passenger.Destination);
            float distance = Utils.CalculateDistancePath(path);
            float profit = _moneyRate * distance;

            _vehiclePassenger.DropPassenger();
            PassengerDelivered?.Invoke(this, profit);
        }

        if (_vehiclePassenger.Passenger != null && _vehiclePassenger.IsInCar == false)
        {
            SetDestination(_vehiclePassenger.Destination);
            _vehiclePassenger.PutInCar();
            PassengerTransportationStarted?.Invoke(this);
        }

        PathCompleted?.Invoke(this);
    }

    private void OnPassengerRefused(Passenger passenger)
    {
        _pathKeeper.ResetPath();
        PathCompleted?.Invoke(this);
    }
}