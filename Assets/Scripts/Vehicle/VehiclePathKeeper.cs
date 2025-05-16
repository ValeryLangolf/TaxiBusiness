using System;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePathKeeper
{
    private const float ReachThreshold = 0.15f;

    private readonly Transform _vehicle;
    private readonly Action _pathCompleted;

    private List<Waypoint> _currentPath = new();
    private List<Waypoint> _remainingPath = new();
    private int _currentTargetIndex;

    public VehiclePathKeeper(Transform vehicle, Action pathCompleted)
    {
        _vehicle = vehicle;
        _pathCompleted = pathCompleted;
    }

    public bool IsActivePath => _currentPath != null && _currentTargetIndex < _currentPath.Count;

    public Vector3 CurrentTarget => GetTarget();

    public Waypoint StartPoint => _currentPath[0];

    public Waypoint EndPoint => _currentPath[^1];

    public List<Waypoint> RemainingPath => new(_remainingPath);

    public void SetPath(List<Waypoint> path)
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

    private void ResetPath()
    {
        _currentPath = null;
        _currentTargetIndex = 0;
    }
}