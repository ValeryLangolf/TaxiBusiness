using System;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle _vehiclePrefab;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _radiusOffset;

    public event Action<Vehicle> Spawned;

    private void Awake()
    {
        if (_vehiclePrefab == null)
            throw new NullReferenceException("_отсутствует ссылка на префаб транспортного средства");
    }

    private void Start()
    {
        SpawnVehicle();
        SpawnVehicle();
    }

    private void SpawnVehicle()
    {
        Vector3 position = _startPosition.position;
        position.x += GetRandomOffset();
        position.y += GetRandomOffset();
        position.z += GetRandomOffset();

        Vehicle vehicle = Instantiate(_vehiclePrefab, position, _startPosition.rotation);
        Spawned?.Invoke(vehicle);
    }

    private float GetRandomOffset() =>
        UnityEngine.Random.Range(-_radiusOffset, _radiusOffset);
}