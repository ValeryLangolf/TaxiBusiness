using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PathDrawerView))]
public class PathDrawer : MonoBehaviour
{
    private PathDrawerView _view;
    [SerializeField] private Color _pathToPassenger;
    [SerializeField] private Color _pathFromPassenger;

    private Vehicle _vehicle;

    private void Awake() =>
        _view = GetComponent<PathDrawerView>();

    private void Update() =>
        Draw();

    private void OnEnable()
    {
        VehicleSelector.Selected += OnVehicleSelected;
        VehicleSelector.Deselected += OnVehicleDeselected;
    }

    private void OnDisable()
    {
        VehicleSelector.Selected -= OnVehicleSelected;
        VehicleSelector.Deselected -= OnVehicleDeselected;
    }

    private void SubscribeVehicle(Vehicle vehicle)
    {
        vehicle.PathDestinated += OnPathDestinated;
        vehicle.PathCompleted += OnPathCompleted;
    }

    private void UnsubscribeVehicle(Vehicle vehicle)
    {
        vehicle.PathDestinated -= OnPathDestinated;
        vehicle.PathCompleted -= OnPathCompleted;
    }

    private void OnVehicleSelected(Vehicle vehicle)
    {
        if (_vehicle == vehicle)
            return;

        _view.Clear();

        if (_vehicle != null)
            UnsubscribeVehicle(_vehicle);

        SubscribeVehicle(vehicle);
        _vehicle = vehicle;
    }

    private void OnVehicleDeselected(Vehicle vehicle)
    {
        UnsubscribeVehicle(vehicle);

        _view.Clear();
        _vehicle = null;
    }

    private void OnPathDestinated(Vehicle vehicle) =>
        _view.Clear();

    private void OnPathCompleted(Vehicle vehicle) =>
        _view.Clear();

    private void Draw()
    {
        if (_vehicle == null || _vehicle.RemainingPath.Count == 0)
            return;

        _view.SetColor(_pathToPassenger);

        List<Vector3> path = new()
        {
            _vehicle.Position
        };

        path.AddRange(_vehicle.RemainingPath.Where(v => v.Position != null).Select(v => v.Position).ToList());
        _view.Draw(path);
    }
}