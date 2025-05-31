using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VehicleParams))]
public class Vehicle : MonoBehaviour
{
    [SerializeField] private VehicleParams _params;

    private Mover _mover;
    private Rotator _rotator;
    private VehiclePathKeeper _pathKeeper;
    private VehiclePassenger _vehiclePassenger;

    public event Action<Vehicle> PathDestinated;
    public event Action<Vehicle> PathCompleted;
    public event Action<Vehicle, float> PassengerDelivered;

    #region Properties
    public VehicleParams Params => _params;

    public Vector3 Position => transform.position;

    public Quaternion Rotation => transform.rotation;

    public bool IsActivePath => _pathKeeper != null && _pathKeeper.IsActivePath;

    public Waypoint EndPoint => _pathKeeper.EndPoint;

    public List<Waypoint> RemainingPath => new(_pathKeeper.RemainingPath);

    public bool IsPassengerAssigned => _vehiclePassenger.IsAssigned;

    public bool IsPassengerInCar => _vehiclePassenger.IsInCar;
    #endregion

    private void Awake()
    {
        _mover = new(transform);
        _rotator = new(transform);
        _vehiclePassenger = new(transform, OnPassengerRefused);
        _pathKeeper = new(transform, OnPathDestinated, OnPathCompleted);
    }

    private void Start() =>
        _pathKeeper.Init(RoadNetwork.Instance.Points);

    private void Update()
    {
        if (_pathKeeper.IsActivePath == false)
            return;

        if (_params.CanGo == false)
        {
            if (_vehiclePassenger.IsInCar)
                _vehiclePassenger.DropPassenger();                
            else if (_vehiclePassenger.IsAssigned)
                _vehiclePassenger.Refuse(_vehiclePassenger.Passenger);

            OnPathCompleted();

            return;
        }

        _mover.Move(_pathKeeper.CurrentTarget, _params.Speed);
        _params.BurnFuel();
        _params.Wear();
        _pathKeeper.UpdatePath();

        if (_pathKeeper.IsActivePath)
            _rotator.Rotate(_pathKeeper.CurrentTarget);
    }

    public void SetPassenger(Passenger passenger) =>
        _vehiclePassenger.AssignPassenger(passenger);

    public void SetDestination(Vector3 destination) =>
        _pathKeeper.SetDestination(destination);

    private void OnPathDestinated() =>
        PathDestinated?.Invoke(this);

    private void OnPathCompleted()
    {
        PathCompleted?.Invoke(this);

        if (_vehiclePassenger.IsAssigned == false)
            return;

        if (IsPassengerInCar == false)
        {
            _vehiclePassenger.PutInCar();
            SetDestination(_vehiclePassenger.Destination);
        }
        else
        {
            float profit = _vehiclePassenger.GetProfit(_params.MoneyRate);
            _vehiclePassenger.DropPassenger();
            PassengerDelivered?.Invoke(this, profit);
        }
    }

    private void OnPassengerRefused()
    {
        _pathKeeper.ResetPath();
        PathCompleted?.Invoke(this);
    }

    private void OnDestroy() =>
        _vehiclePassenger.ProcessDestroyed();
}