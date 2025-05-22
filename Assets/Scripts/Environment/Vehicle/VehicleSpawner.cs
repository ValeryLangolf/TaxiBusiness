using System;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _radiusOffset;

    public event Action<Vehicle> Spawned;

    public void Spawn(VehicleConfig vehicleSO)
    {
        Vector3 position = _startPosition.position;
        position.x += GetRandomOffset();
        position.z += GetRandomOffset();

        Vehicle vehicle = Instantiate(vehicleSO.Prefab, position, _startPosition.rotation);
        vehicle.InitParams(vehicleSO);
        Spawned?.Invoke(vehicle);
    }

    private float GetRandomOffset() =>
        UnityEngine.Random.Range(-_radiusOffset, _radiusOffset);
}