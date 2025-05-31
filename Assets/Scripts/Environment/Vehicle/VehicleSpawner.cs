using System;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _radiusOffset;

    public event Action<Vehicle> Spawned;

    public void Spawn(Vehicle vehiclePrefab)
    {
        Vector3 position = _startPosition.position;
        position.x += GetRandomOffset();
        position.z += GetRandomOffset();

        Spawn(vehiclePrefab, position, _startPosition.rotation);
    }

    public Vehicle Spawn(Vehicle vehiclePrefab, Vector3 position, Quaternion rotation)
    {
        Vehicle vehicle = Instantiate(vehiclePrefab, position, rotation);
        Spawned?.Invoke(vehicle);

        return vehicle;
    }

    private float GetRandomOffset() =>
        UnityEngine.Random.Range(-_radiusOffset, _radiusOffset);
}