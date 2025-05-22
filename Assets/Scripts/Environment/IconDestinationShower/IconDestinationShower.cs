using UnityEngine;

public class IconDestinationShower : MonoBehaviour
{
    [SerializeField] private VehicleSelector _selector;

    private IconDestinationShowerView _view;
    private Vehicle _vehicle;
    private UiFollower _follower;

    private void Awake()
    {
        _view = GetComponentInChildren<IconDestinationShowerView>(true);
        _follower = GetComponentInChildren<UiFollower>();

        _view.Hide();

        _selector.Selected += OnVehicleSelected;
        _selector.Deselected += OnVehicleDeselected;
    }

    private void OnDestroy()
    {
        _selector.Selected -= OnVehicleSelected;
        _selector.Deselected -= OnVehicleDeselected;

        UnsubscribeVehicle(_vehicle);
    }

    private void SubscribeVehicle(Vehicle vehicle)
    {
        if (_vehicle == vehicle)
            return;

        if (_vehicle != null)
            UnsubscribeVehicle(_vehicle);

        vehicle.PathDestinated += OnPathDestination;
        vehicle.PathCompleted += OnPathComplete;

        _vehicle = vehicle;
    }

    private void UnsubscribeVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
            return;

        vehicle.PathDestinated -= OnPathDestination;
        vehicle.PathCompleted -= OnPathComplete;

        _vehicle = null;
    }

    private void OnVehicleSelected(Vehicle vehicle)
    {
        SubscribeVehicle(vehicle);

        if (_vehicle.IsActivePath)
            OnPathDestination(_vehicle);
    }

    private void OnVehicleDeselected(Vehicle vehicle)
    {
        UnsubscribeVehicle(vehicle);
        _view.Hide();
    }

    private void OnPathDestination(Vehicle vehicle)
    {
        _view.Show();

        if (vehicle.IsPassengerInCar)
        {
            _view.SetPassengerFinishIcon();
        }
        else if (vehicle.IsPassengerAssigned)
        {
            _view.Hide();
        }
        else
        {
            _view.SetDefaultIcon();
        }

        _follower.Follow(vehicle.EndPoint.transform);
    }

    private void OnPathComplete(Vehicle _) =>
        _view.Hide();
}