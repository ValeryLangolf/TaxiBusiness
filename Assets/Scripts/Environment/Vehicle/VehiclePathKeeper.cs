using System;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePathKeeper
{
    private const float ReachThreshold = 0.15f;

    private readonly Transform _vehicle;
    private readonly Action _pathDestinated;
    private readonly Action _pathCompleted;

    private List<Waypoint> _points;
    private List<Waypoint> _currentPath = new();
    private List<Waypoint> _remainingPath = new();
    private int _currentTargetIndex;

    public VehiclePathKeeper(Transform vehicle, Action pathDestinated, Action pathCompleted)
    {
        _vehicle = vehicle;
        _pathDestinated = pathDestinated;
        _pathCompleted = pathCompleted;

        _points = RoadNetwork.Instance.Points;

        if(_points == null)
            throw new ArgumentNullException("Неудачная инициализация списка точек на карте");

        if (_points.Count == 0)
            throw new ArgumentNullException("Нет доступных точек маршрута");
    }

    public bool IsActivePath => _currentPath != null && _currentTargetIndex < _currentPath.Count;

    public Vector3 CurrentTarget => GetTarget();

    public Waypoint StartPoint => _currentPath[0];

    public Waypoint EndPoint => _currentPath[^1];

    public List<Waypoint> RemainingPath => new(_remainingPath);

    public void SetDestination(Vector3 destination)
    {
        Waypoint start = Utils.GetNearestSectionAndPoint(_vehicle.position, _points);
        Waypoint end = Utils.GetNearestSectionAndPoint(destination, _points);

        if (start == null)
            throw new ArgumentNullException("Не удалось получить ближайшую стартовую точку пути");

        if (end == null)
            throw new ArgumentNullException("Не удалось получить ближайшую конечную точку пути");

        if (start == end)
        {
            Debug.LogWarning("Стартовая точка в то же время оказалась и конечной");

            return;
        }

        List<Waypoint> path = Pathfinder.FindPath(start, end);
        SetPath(path ?? new List<Waypoint>());

        if (path?.Count > 0)
            _pathDestinated?.Invoke();
    }

    private void SetPath(List<Waypoint> path)
    {
        if (path == null)
        {
            Debug.LogWarning("Авто получило нулевой путь");

            return;
        }

        _currentTargetIndex = 0;
        _currentPath = new(path);
        _remainingPath = new(path);
    }

    public void ResetPath()
    {
        _currentPath.Clear();
        _remainingPath.Clear();
        _currentTargetIndex = 0;
    }

    public void UpdatePath()
    {
        if (IsActivePath == false)
        {
            ResetPath();

            return;
        }

        while ((Vector3.Distance(_vehicle.position, GetTarget()) < ReachThreshold) && IsActivePath)
        {
            _currentTargetIndex++;

            if (_remainingPath.Count > 0)
                _remainingPath.RemoveAt(0);

            if (_currentTargetIndex >= _currentPath.Count)
                _pathCompleted?.Invoke();
        }
    }

    private Vector3 GetTarget()
    {
        if (IsActivePath == false)
            return Vector3.zero;

        Vector3 target = _currentPath[_currentTargetIndex].Position;
        target.y = _vehicle.position.y;

        return target;
    }
}