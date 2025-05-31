using System;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _radiusOffset;

    public event Action<Vehicle> Spawned;

    public Vehicle Spawn(Vehicle vehiclePrefab, Vector3 position, Quaternion rotation)
    {
        position.x += GetRandomOffset();
        position.z += GetRandomOffset();

        Vehicle vehicle = Instantiate(vehiclePrefab, position, rotation);
        Spawned?.Invoke(vehicle);

        return vehicle;
    }

    private float GetRandomOffset() =>
        UnityEngine.Random.Range(-_radiusOffset, _radiusOffset);
}