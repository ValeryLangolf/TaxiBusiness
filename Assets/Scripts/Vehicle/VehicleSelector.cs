using System;
using UnityEngine;

public class VehicleSelector : MonoBehaviour
{
    private static Vehicle s_selectedVehicle;

    public static event Action<Vehicle> Selected;
    public static event Action Deselected;

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

    private void HandleRightClick(Collider _, Vector3 __) =>
        Deselect();

    private void Select(VehicleCollider collider)
    {
        s_selectedVehicle = collider.Vehicle;
        Selected?.Invoke(collider.Vehicle);
    }

    private void Deselect()
    {
        s_selectedVehicle = null;
        Deselected?.Invoke();
    }
}
