using System;
using UnityEngine;

public class VehicleDispatcher : MonoBehaviour
{
    public static event Action<Vector3> PlaneClicked;

    private void OnEnable()
    {
        MouseHitInformer.PlaneLeftClicked += OnPlaneClicked;
        MouseHitInformer.PassengerClicked += OnClickPassenger;
    }

    private void OnDisable()
    {
        MouseHitInformer.PlaneLeftClicked -= OnPlaneClicked;
        MouseHitInformer.PassengerClicked -= OnClickPassenger;
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
        Vehicle vehicle = VehicleSelector.Vehicle;

        if (vehicle == null)
            return;

        if (vehicle.IsPassengerInCar)
            return;

        vehicle.ResetPassenger();
        vehicle.SetDestination(position);

        if (passenger != null)
            vehicle.SetWaitingPassenger(passenger);
    }
}