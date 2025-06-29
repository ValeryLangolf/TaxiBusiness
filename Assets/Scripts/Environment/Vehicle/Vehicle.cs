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
    private DriverMan _driverMan;

    #region Events
    public event Action<Vehicle> PathDestinated;
    public event Action<Vehicle> PathCompleted;
    public event Action<Vehicle, float> PassengerDelivered;
    public event Action<float> MoneySpended;
    public event Action<Vehicle> Destroyed;
    #endregion

    #region Properties
    public VehicleParams Params => _params;

    public Vector3 Position => transform.position;

    public Quaternion Rotation => transform.rotation;

    public bool IsActivePath => _pathKeeper != null && _pathKeeper.IsActivePath;

    public Waypoint EndPoint => _pathKeeper.EndPoint;

    public List<Waypoint> RemainingPath => new(_pathKeeper.RemainingPath);

    public bool IsPassengerAssigned => _vehiclePassenger.IsAssigned;

    public bool IsPassengerInCar => _vehiclePassenger.IsInCar;

    public DriverMan DriverMan => _driverMan;
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
        FillFuelAutomatically();
        FillRepairAutomatically();

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

    public void SetDriverMan(DriverMan driverMan) =>
        _driverMan = driverMan;

    public void ResetDriverMan()
    {
        Destroy(_driverMan.gameObject);
        _driverMan = null;
    }

    private void FillFuelAutomatically()
    {
        if (_driverMan == null)
            return;

        if(_params.TryFillFuel(Constants.FuelFillingSpeed * Time.deltaTime) == false)
            return;

        MoneySpended?.Invoke(FuelParams.CurrentPrice * Time.deltaTime);
    }

    private void FillRepairAutomatically()
    {
        if (_driverMan == null)
            return;

        if (_params.TryFillRepair(Constants.RepairFillingSpeed * Time.deltaTime) == false)
            return;

        MoneySpended?.Invoke(RepairParams.CurrentPrice * Time.deltaTime);
    }

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

            if (_driverMan != null)
                profit -= profit * _driverMan.ShareOfRevenue;

            _vehiclePassenger.DropPassenger();
            PassengerDelivered?.Invoke(this, profit);
        }
    }

    private void OnPassengerRefused()
    {
        _pathKeeper.ResetPath();
        PathCompleted?.Invoke(this);
    }

    private void OnDestroy()
    {
        _vehiclePassenger.ProcessDestroyed();
        Destroyed?.Invoke(this);
    }
}