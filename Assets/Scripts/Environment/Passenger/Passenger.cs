using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UiFollower))]
public class Passenger : MonoBehaviour, IDeactivatable<Passenger>
{
    [SerializeField] private PassengerView _view;
    [SerializeField] private Vector2 _timeVisibleLimits;

    private Waypoint _departurePoint;
    private Waypoint _destinationPoint;
    private UiFollower _follower;
    private Coroutine _coroutine;

    private readonly List<VehiclePassenger> _sentVehicles = new();

    public event Action<Passenger> Deactivated;
    public event Action<Passenger> Taked;

    public Transform Target => _follower.Target;

    public Waypoint DeparturePoint => _departurePoint;

    public Waypoint DestinationPoint => _destinationPoint;

    public bool IsSelect => _sentVehicles.Count > 0;

    private void Awake()
    {
        _follower = GetComponent<UiFollower>();
        _view.AnimatationEnded += OnAnimationEnded;
    }

    private void OnDestroy() =>
        _view.AnimatationEnded -= OnAnimationEnded;

    private void OnEnable()
    {
        _view.SetDefaultIconSize();
        _coroutine = StartCoroutine(HideOverTime());
        SfxPlayer.Instance.PlayPassengerShowing();
    }

    public void Follow(Transform target) =>
        _follower.Follow(target);

    public void PickUp(VehiclePassenger vehicle)
    {
        if (enabled)
            StopCoroutine(_coroutine);

        _view.SetMiniIconSize();
        Follow(vehicle.Transform);

        SfxPlayer.Instance.PlayPassengerGoInCar();

        List<VehiclePassenger> sentVehicles = new(_sentVehicles);

        foreach (VehiclePassenger sentVehicle in sentVehicles)
        {
            if (vehicle == sentVehicle)
                continue;

            sentVehicle.Refuse(this);
            _sentVehicles.Remove(sentVehicle);
        }

        Taked?.Invoke(this);
    }

    public void SetDeparture(Waypoint point)
    {
        _departurePoint = point;
        Follow(point.transform);
    }

    public void SetDestination(Waypoint point) =>
        _destinationPoint = point;

    public void AcceptOrder(VehiclePassenger vehicle)
    {
        if(vehicle == null)
            return;

        _sentVehicles.Add(vehicle);
        _view.Select();
    }

    public void CancelOrder(VehiclePassenger vehicle)
    {
        _sentVehicles.Remove(vehicle);

        if (_sentVehicles.Count == 0)
            _view.Deselect();
    }

    public void ReturnInPool() =>
        Deactivated?.Invoke(this);

    private IEnumerator HideOverTime()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_timeVisibleLimits.x, _timeVisibleLimits.y));

        _view.AnimateHidding();
        RefuseAllVehicles();        
    }

    private void RefuseAllVehicles()
    {
        List<VehiclePassenger> sentVehicles = new(_sentVehicles);

        foreach (VehiclePassenger vehicle in sentVehicles)
            vehicle.Refuse(this);
    }

    private void OnAnimationEnded() =>
        ReturnInPool();    
}