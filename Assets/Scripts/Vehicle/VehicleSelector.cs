using System;
using UnityEngine;

public class VehicleSelector : MonoBehaviour
{
    private static Vehicle s_selectedVehicle;

    public static event Action<Vehicle> Selected;
    public static event Action<Vehicle> Deselected;

    public static Vehicle Vehicle => s_selectedVehicle;

    private void OnEnable()
    {
        MouseHitInformer.LeftHitted += HandleLeftClick;
        MouseHitInformer.RightHitted += HandleRightClick;
    }

    private void OnDisable()
    {
        MouseHitInformer.LeftHitted -= HandleLeftClick;
        MouseHitInformer.RightHitted -= HandleRightClick;
    }

    private void HandleLeftClick(Collider collider, Vector3 hitPoint)
    {
        if (collider.TryGetComponent(out VehicleCollider vehicleCollider))
            Select(vehicleCollider);
    }

    private void HandleRightClick(Collider _, Vector3 __)
    {
        Deselect(s_selectedVehicle);
        s_selectedVehicle = null;
    }

    private void Select(VehicleCollider collider)
    {
        Vehicle vehicle = collider.Vehicle;

        if(s_selectedVehicle != null && s_selectedVehicle != vehicle)
            Deselect(s_selectedVehicle);

        s_selectedVehicle = vehicle;
        Selected?.Invoke(collider.Vehicle);
    }

    private void Deselect(Vehicle vehicle) =>
        Deselected?.Invoke(vehicle);
}