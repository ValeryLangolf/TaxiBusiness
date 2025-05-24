using System;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    #region SerializedFields
    [Header("Параметры автомобиля")]

    [Tooltip("Название авто \n(так отображается название в карточке магазина)")]
    [SerializeField]
    private string _name;

    [Tooltip("Краткое описание авто, плюсы и минусы")]
    [SerializeField, TextArea(3, 10)]
    private string _description;

    [Tooltip("Отображается в карточках магазина и иконках авто")]
    [SerializeField] private Sprite _sprite;

    [Tooltip("Скорость авто \n(не соответствует км/ч)")]
    [SerializeField, Range(Constants.MinSpeed, Constants.MaxSpeed)]
    private float _speed;

    [Tooltip("Износостойкость \n(чем выше значение, тем медленнее износ)")]
    [SerializeField, Range(Constants.MinWearResistance, Constants.MaxWearResistance)]
    private float _wearResistance;

    [Tooltip("Экономичность топлива \n(чем выше значение, тем дольше расходуется топливо)")]
    [SerializeField, Range(Constants.MinFuelEfficiency, Constants.MaxFuelEfficiency)]
    private float _fuelEfficiency;

    [Tooltip("Изначальная стоимость авто в магазине \n(может меняться по мере прокачки для последующей продажи)")]
    [SerializeField]
    private float _price;

    [Tooltip("Коэффициент, влияющий на заработок. \n(чем выше значение, тем выше доход с поездки)")]
    [SerializeField, Range(0f, 1f)]
    private float _moneyRate;
    #endregion

    private Mover _mover;
    private Rotator _rotator;
    private VehiclePathKeeper _pathKeeper;
    private VehiclePassenger _vehiclePassenger;

    public event Action<Vehicle> PathDestinated;
    public event Action<Vehicle> PathCompleted;
    public event Action<Vehicle, float> PassengerDelivered;

    #region Properties
    public float MoneyRate => _moneyRate;

    public string Name => _name;

    public Sprite Sprite => _sprite;

    public float Speed => _speed;

    public float WearResistance => _wearResistance;

    public float FuelEfficiency => _fuelEfficiency;

    public string Description => _description;

    public float Price => _price;

    public Vector3 Position => transform.position;

    public Quaternion Rotation => transform.rotation;

    public bool IsActivePath => _pathKeeper.IsActivePath;

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
    }

    private void Start() =>
        _pathKeeper = new(transform, OnPathDestinated, OnPathCompleted);

    private void Update()
    {
        if (_pathKeeper.IsActivePath == false)
            return;

        _mover.Move(_pathKeeper.CurrentTarget, _speed);
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
            float profit = _vehiclePassenger.GetProfit(_moneyRate);
            _vehiclePassenger.DropPassenger();
            PassengerDelivered?.Invoke(this, profit);
        }
    }

    private void OnPassengerRefused()
    {
        _pathKeeper.ResetPath();
        PathCompleted?.Invoke(this);
    }
}