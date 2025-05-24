using UnityEngine;

public class ProfitShower : MonoBehaviour
{
    [SerializeField] private ProfitView _profitViewPrefab;
    [SerializeField] private VehicleSpawner _vehicleSpawner;

    private Pool<ProfitView> _pool;

    private void Awake() =>
        _pool = new(_profitViewPrefab, transform);

    private void OnEnable() =>
        _vehicleSpawner.Spawned += OnVehicleSpawned;

    private void OnDisable() =>
        _vehicleSpawner.Spawned -= OnVehicleSpawned;

    private void OnVehicleSpawned(Vehicle vehicle) =>
        vehicle.PassengerDelivered += OnPassengerDelivered;

    private void OnPassengerDelivered(Vehicle vehicle, float profit)
    {
        if (_pool.TryGet(out ProfitView profitView) == false)
            return;

        profitView.SetText(profit);
        profitView.SetPositionUi(vehicle.Position);
    }
}