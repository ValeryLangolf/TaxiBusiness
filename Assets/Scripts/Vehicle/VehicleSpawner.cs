using System;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle _vehiclePrefab;
    [SerializeField] private Transform _startPosition;

    private void Awake()
    {
        if (_vehiclePrefab == null)
            throw new NullReferenceException("_отсутствует ссылка на префаб транспортного средства");
    }

    private void Start() =>
        SpawnVehicle();

    private void SpawnVehicle() =>
        Instantiate(_vehiclePrefab, _startPosition.position, _startPosition.rotation);
}