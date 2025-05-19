using System;
using UnityEngine;

public class VehicleSelector : MonoBehaviour
{
    [SerializeField] private MouseHitInformer _mouseHitInformer;

    private Vehicle _selectedVehicle;

    public event Action<Vehicle> Selected;
    public event Action<Vehicle> Deselected;

    public Vehicle Vehicle => _selectedVehicle;

    private void OnEnable()
    {
        _mouseHitInformer.VehicleClicked += HandleLeftClick;
        _mouseHitInformer.RightHitted += HandleRightClick;
    }

    private void OnDisable()
    {
        _mouseHitInformer.VehicleClicked -= HandleLeftClick;
        _mouseHitInformer.RightHitted -= HandleRightClick;
    }

    private void HandleLeftClick(Vehicle vehicle) =>
        Select(vehicle);

    private void HandleRightClick(Collider _, Vector3 __)
    {
        Deselect(_selectedVehicle);
        _selectedVehicle = null;
    }

    private void Select(Vehicle vehicle)
    {
        if (_selectedVehicle != null && _selectedVehicle != vehicle)
            Deselect(_selectedVehicle);

        _selectedVehicle = vehicle;
        Selected?.Invoke(vehicle);
    }

    private void Deselect(Vehicle vehicle) =>
        Deselected?.Invoke(vehicle);
}