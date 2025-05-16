using UnityEngine;

[RequireComponent(typeof(UiFollower))]
public class IconStartShower : MonoBehaviour
{
    private Vehicle _vehicle;
    private UiFollower _follower;

    private void Awake()
    {
        _follower = GetComponent<UiFollower>();

        VehicleSelector.Selected += HandleVehicleSelected;
        VehicleSelector.Deselected += HandleDeselected;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        VehicleSelector.Selected -= HandleVehicleSelected;
        VehicleSelector.Deselected -= HandleDeselected;

        if (_vehicle != null)
            UnsubscribeVehicle(_vehicle);
    }

    private void SubscribeVehicle(Vehicle vehicle)
    {
        vehicle.PathDestinated += HandlePathDestinated;
        vehicle.PathCompleted += HandlePathCompleted;
    }

    private void UnsubscribeVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
            return;

        _vehicle = null;
        vehicle.PathDestinated -= HandlePathDestinated;
        vehicle.PathCompleted -= HandlePathCompleted;        
    }

    private void HandleVehicleSelected(Vehicle vehicle)
    {
        if (vehicle == _vehicle)
            return;

        SwitchTrackedVehicle(vehicle);
        UpdateIconState();
    }

    private void SwitchTrackedVehicle(Vehicle vehicle)
    {
        UnsubscribeVehicle(_vehicle);
        _vehicle = vehicle;
        SubscribeVehicle(_vehicle);
    }

    private void UpdateIconState()
    {
        if (_vehicle.IsActivePath)
            HandlePathDestinated(_vehicle);
    }

    private void HandleDeselected(Vehicle vehicle)
    {
        UnsubscribeVehicle(vehicle);
        gameObject.SetActive(false);
    }

    private void HandlePathDestinated(Vehicle vehicle)
    {
        gameObject.SetActive(true);
        _follower.Follow(vehicle.StartPoint.transform);
    }

    private void HandlePathCompleted(Vehicle _) =>
        gameObject.SetActive(false);
}