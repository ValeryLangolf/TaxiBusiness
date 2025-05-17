using System;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle _vehiclePrefab;
    [SerializeField] private Transform _startPosition;

    public event Action<Vehicle> Spawned;

    private void Awake()
    {
        if (_vehiclePrefab == null)
            throw new NullReferenceException("_отсутствует ссылка на префаб транспортного средства");
    }

    private void Start() =>
        SpawnVehicle();

    private void SpawnVehicle()
    {
        Vehicle vehicle = Instantiate(_vehiclePrefab, _startPosition.position, _startPosition.rotation);
        Spawned?.Invoke(vehicle);
    }
}