using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDispatcher : MonoBehaviour
{
    private List<Waypoint> _points;

    public static event Action<Vector3> PlaneClicked;

    private void OnEnable() =>
        MouseHitInformer.LeftHitted += HandleHitted;

    private void OnDisable() =>
        MouseHitInformer.LeftHitted -= HandleHitted;

    private void Start() =>
        _points = RoadNetwork.Instance.Points;

    private void HandleHitted(Collider collider, Vector3 hitPoint)
    {
        Vehicle vehicle = VehicleSelector.Vehicle;

        if (vehicle == null)
            return;

        if (collider.TryGetComponent(out Plane _) == false)
            return;

        SendToDestination(vehicle, hitPoint);
        PlaneClicked?.Invoke(hitPoint);
    }

    private void SendToDestination(Vehicle vehicle, Vector3 destination)
    {
        Waypoint start = Utils.GetNearestSectionAndPoint(vehicle.Position, _points);
        Waypoint end = Utils.GetNearestSectionAndPoint(destination, _points);

        if (start == end)
            return;

        List<Waypoint> path = Pathfinder.FindPath(start, end);

        if(path == null)
        {
            Debug.Log($"Твой путь Null");
            return;
        }

        if (path.Count == 0)
        {
            Debug.Log($"Для пути не найдено ни одной точки");
            return;
        }

        vehicle.SetPath(path);
    }
}