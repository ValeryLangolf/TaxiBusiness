using UnityEngine;

public class DriverManSpawner : MonoBehaviour
{
    [SerializeField] private DriverMan _driverManPrefab;

    public DriverMan Spawn() =>
        Instantiate(_driverManPrefab, transform);
}