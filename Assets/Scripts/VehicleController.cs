using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private List<Vector3> _currentPath;
    private int _currentWaypointIndex;
    private bool _isMoving;

    public event Action PathCompleted;

    private void Update()
    {
        if (_currentPath == null || _currentPath.Count == 0)
            return;

        Vector3 targetPosition = _currentPath[_currentWaypointIndex];
        MoveTowards(targetPosition);

        if (Vector3.Distance(transform.position, targetPosition) < Constants.MinDistanceToWaypoint)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _currentPath.Count)
                CompletePath();
        }
    }

    public void SetPath(List<Vector3> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.LogError("Получен пустой путь");
            return;
        }

        _currentPath = path;
        _currentWaypointIndex = 0;
        _isMoving = true;

        Debug.Log($"Назначен новый путь с {path.Count} точками");
    }

    public bool CheckIsMoving() => _isMoving;

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotation,
            Time.deltaTime * _rotationSpeed
        );

        transform.position += _speed * Time.deltaTime * transform.forward;
    }

    private void CompletePath()
    {
        _currentPath = null;
        _currentWaypointIndex = 0;
        _isMoving = false;
        PathCompleted?.Invoke();
    }
}