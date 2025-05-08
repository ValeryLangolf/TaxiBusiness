using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private RoadManager _roadManager;
    [SerializeField] private Pathfinder _pathfinder;
    [SerializeField] private VehicleController _vehiclePrefab;

    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _defaultDestination;
    [SerializeField] private Transform _customStartPoint;
    [SerializeField] private Transform _customEndPoint;

    [SerializeField] private float _spawnInterval = 3f;

    private void Awake()
    {
        if (_roadManager == null)
            throw new NullReferenceException("_roadManager отсутствует ссылка на компонент");

        if (_pathfinder == null)
            throw new NullReferenceException("_pathfinder отсутствует ссылка на компонент");

        if (_vehiclePrefab == null)
            throw new NullReferenceException("_vehiclePrefab отсутствует ссылка на компонент");
    }

    private void Start() =>
        InvokeRepeating(nameof(SpawnVehicle), 0f, _spawnInterval);

    private void SpawnVehicle()
    {
        VehicleController vehicle = Instantiate(_vehiclePrefab, _startPosition.position, _startPosition.rotation);

        var (startLane, startPoint) = GetNearestLaneAndPoint(_customStartPoint.position);
        var (endLane, endPoint) = GetNearestLaneAndPoint(_customEndPoint.position);

        if (startLane == null || endLane == null)
            return;

        List<Vector3> path = GetPathBetweenPoints(startLane, startPoint, endLane, endPoint);

        if (path.Count == 0)
            return;

        vehicle.SetPath(path);
    }

    private (SectionRoadStrip, Transform) GetNearestLaneAndPoint(Vector3 position)
    {
        SectionRoadStrip nearestLane = null;
        Transform nearestPoint = null;
        float minDistance = Mathf.Infinity;

        foreach (SectionRoadStrip lane in _roadManager.GetAllLanes())
        {
            Transform point = lane.GetClosestPoint(position);
            float distance = Vector3.Distance(position, point.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestLane = lane;
                nearestPoint = point;
            }
        }

        return (nearestLane, nearestPoint);
    }

    private List<Vector3> GetPathBetweenPoints(
        SectionRoadStrip startLane,
        Transform startPoint,
        SectionRoadStrip endLane,
        Transform endPoint)
    {
        List<SectionRoadStrip> lanePath = _pathfinder.FindPath(startLane, endLane);
        List<Vector3> path = new();

        int startIndex = startLane.GetPointIndex(startPoint);

        for (int i = startIndex; i < startLane.Points.Count; i++)
            path.Add(startLane.Points[i].position);

        for (int i = 1; i < lanePath.Count - 1; i++)
            path.AddRange(lanePath[i].Points.Select(p => p.position));

        int endIndex = endLane.GetPointIndex(endPoint);

        for (int i = 0; i <= endIndex; i++)
            path.Add(endLane.Points[i].position);

        return path;
    }
}