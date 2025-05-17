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
        MouseHitInformer.VehicleClicked += HandleLeftClick;
        MouseHitInformer.RightHitted += HandleRightClick;
    }

    private void OnDisable()
    {
        MouseHitInformer.VehicleClicked -= HandleLeftClick;
        MouseHitInformer.RightHitted -= HandleRightClick;
    }

    private void HandleLeftClick(Vehicle vehicle) =>
        Select(vehicle);

    private void HandleRightClick(Collider _, Vector3 __)
    {
        Deselect(s_selectedVehicle);
        s_selectedVehicle = null;
    }

    private void Select(Vehicle vehicle)
    {
        if (s_selectedVehicle != null && s_selectedVehicle != vehicle)
            Deselect(s_selectedVehicle);

        s_selectedVehicle = vehicle;
        Selected?.Invoke(vehicle);
    }

    private void Deselect(Vehicle vehicle) =>
        Deselected?.Invoke(vehicle);
}