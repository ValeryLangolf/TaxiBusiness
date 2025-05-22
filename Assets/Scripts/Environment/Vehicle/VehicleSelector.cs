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
        _mouseHitInformer.VehicleClicked += OnLeftClick;
        _mouseHitInformer.RightHitted += OnRightClick;
    }

    private void OnDisable()
    {
        _mouseHitInformer.VehicleClicked -= OnLeftClick;
        _mouseHitInformer.RightHitted -= OnRightClick;
    }

    public void Select(Vehicle vehicle)
    {
        if (_selectedVehicle != null && _selectedVehicle != vehicle)
            Deselect(_selectedVehicle);

        _selectedVehicle = vehicle;
        Selected?.Invoke(vehicle);
        SfxPlayer.Instance.PlayVehicleSelected();
    }

    private void OnLeftClick(Vehicle vehicle) =>
        Select(vehicle);

    private void OnRightClick(Collider _, Vector3 __)
    {
        Deselect(_selectedVehicle);
        _selectedVehicle = null;
    }

    private void Deselect(Vehicle vehicle) =>
        Deselected?.Invoke(vehicle);
}