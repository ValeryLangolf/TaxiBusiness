using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _reachThreshold = 1f;

    private List<Transform> _currentPath;
    private int _currentTargetIndex;

    public void SetPath(List<Transform> path)
    {
        _currentPath = path;
        _currentTargetIndex = 0;
    }

    private void Update()
    {
        if (_currentPath == null || _currentTargetIndex >= _currentPath.Count)
            return;

        Vector3 target = _currentPath[_currentTargetIndex].position;
        MoveTowards(target);

        if (Vector3.Distance(transform.position, target) < _reachThreshold)
            _currentTargetIndex++;
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * _rotationSpeed
        );

        transform.position += _speed * Time.deltaTime * transform.forward;
    }
}