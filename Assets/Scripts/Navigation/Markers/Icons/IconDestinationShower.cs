using UnityEngine;

[RequireComponent(typeof(UiFollower))]
public class IconDestinationShower : MonoBehaviour
{
    private IconDestinationShowerView _view;
    private UiFollower _follower;
    private Vehicle _vehicle;

    private void Awake()
    {
        _view = GetComponentInChildren<IconDestinationShowerView>(true);
        _follower = GetComponent<UiFollower>();

        VehicleSelector.Selected += OnVehicleSelected;
        VehicleSelector.Deselected += OnVehicleDeselected;
        _view.Hide();
    }

    private void OnDestroy()
    {
        VehicleSelector.Selected -= OnVehicleSelected;
        VehicleSelector.Deselected -= OnVehicleDeselected;

        if (_vehicle != null)
            UnsubscribeVehicle(_vehicle);
    }

    private void SubscribeVehicle(Vehicle vehicle)
    {
        vehicle.PathDestinated += OnPathDestinated;
        vehicle.PathCompleted += OnPathCompleted;
        vehicle.PassengerTransportationStarted += OnPassengerInCar;
    }

    private void UnsubscribeVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
            return;

        _vehicle = null;
        vehicle.PathDestinated -= OnPathDestinated;
        vehicle.PathCompleted -= OnPathCompleted;
        vehicle.PassengerTransportationStarted -= OnPassengerInCar;
    }

    private void OnVehicleSelected(Vehicle vehicle)
    {
        if (vehicle.IsActivePath)
            OnPathDestinated(vehicle);

        if (vehicle == _vehicle)
            return;

        SwitchTrackedVehicle(vehicle);
    }

    private void SwitchTrackedVehicle(Vehicle vehicle)
    {
        UnsubscribeVehicle(_vehicle);
        SubscribeVehicle(vehicle);

        _vehicle = vehicle;
    }

    private void OnVehicleDeselected(Vehicle vehicle)
    {
        UnsubscribeVehicle(vehicle);
        _view.Hide();
    }

    private void OnPathDestinated(Vehicle vehicle)
    {
        _view.Show();

        if (vehicle.IsPassengerInCar)
            _view.SetPassengerFinishIcon();
        else
            _view.SetDefaultIcon();


        _follower.Follow(vehicle.EndPoint.transform);
    }

    private void OnPathCompleted(Vehicle _)
    {
        _view.Hide();
        _follower.Stop();
    }

    private void OnPassengerInCar(Vehicle vehicle) =>
        _view.SetPassengerFinishIcon();
}