using System;
using UnityEngine;

public class VehicleDispatcher : MonoBehaviour
{
    [SerializeField] private VehicleSelector _selector;
    [SerializeField] private MouseHitInformer _clickInformer;

    public event Action<Vector3> PlaneClicked;

    private void OnEnable()
    {
        _clickInformer.PlaneLeftClicked += OnPlaneClicked;
        _clickInformer.PassengerClicked += OnClickPassenger;
    }

    private void OnDisable()
    {
        _clickInformer.PlaneLeftClicked -= OnPlaneClicked;
        _clickInformer.PassengerClicked -= OnClickPassenger;
    }

    private void OnPlaneClicked(Vector3 position)
    {
        HandleClick(position, null);
        PlaneClicked?.Invoke(position);
    }

    private void OnClickPassenger(Passenger passenger)
    {
        HandleClick(passenger.Point.position, passenger);
        PlaneClicked?.Invoke(passenger.Point.transform.position);
    }

    private void HandleClick(Vector3 position, Passenger passenger)
    {
        Vehicle vehicle = _selector.Vehicle;

        if (vehicle == null)
            return;

        if (vehicle.IsPassengerInCar)
            return;

        if (passenger != null)
            vehicle.AssignPassenger(passenger);

        vehicle.SetDestination(position);

        SfxPlayer.Instance.PlayEngineRoar();
    }
}