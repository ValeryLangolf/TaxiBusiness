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
        Vehicle vehicle = VehicleSelector.Vehicle;

        if (vehicle == null)
            return;

        if (vehicle.IsPassengerInCar)
            return;

        vehicle.ResetPassenger();

        vehicle.SetDestination(position);
        PlaneClicked?.Invoke(position);
    }

    private void OnClickPassenger(Passenger passenger)
    {
        Vehicle vehicle = VehicleSelector.Vehicle;

        if (vehicle == null)
            return;

        if (vehicle.IsPassengerInCar)
            return;

        vehicle.ResetPassenger();

        vehicle.SetDestination(passenger.Point.position);
        vehicle.SetWaitingPassenger(passenger);
        PlaneClicked?.Invoke(passenger.Point.transform.position);
    }
}