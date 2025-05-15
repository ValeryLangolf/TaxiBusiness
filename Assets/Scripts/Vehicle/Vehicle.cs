using System;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Mover _mover;
    private Rotator _rotator;
    private VehiclePathKeeper _pathKeeper;

    public event Action PathDestinated;
    public event Action PathCompleted;

    public bool IsActivePath => _pathKeeper.IsActivePath;

    public Waypoint StartPoint => _pathKeeper.StartPoint;

    public Waypoint EndPoint => _pathKeeper.EndPoint;

    private void Awake()
    {
        _mover = new(transform, _speed);
        _rotator = new(transform);
        _pathKeeper = new(transform, OnPathCompleted);
    }

    private void Update()
    {
        if (_pathKeeper.IsActivePath == false)
            return;

        _mover.Move(_pathKeeper.CurrentTarget);
        _pathKeeper.UpdatePath();

        if (_pathKeeper.IsActivePath)
            _rotator.Rotate(_pathKeeper.CurrentTarget);
    }

    public void SetPath(List<Waypoint> path)
    {
        _pathKeeper.SetPath(path);
        PathDestinated?.Invoke();
    }

    private void OnPathCompleted() =>
        PathCompleted?.Invoke();
}