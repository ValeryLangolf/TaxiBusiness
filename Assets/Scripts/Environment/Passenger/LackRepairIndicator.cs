using UnityEngine;

public class LackRepairIndicator : MonoBehaviour
{
    [SerializeField] private UiFollower _follower;

    private VehicleParams _vehicleParams;

    public void SetVehicle(VehicleParams vehicleParams)
    {
        if (vehicleParams == null)
            Destroy(gameObject);

        _vehicleParams = vehicleParams;
        _follower.Follow(vehicleParams.transform);
        Subscribe();
    }

    private void Subscribe()
    {
        _vehicleParams.RepairChanged += OnRepairChanged;
        _vehicleParams.Destroyed += OnVehicleDestroyed;
    }

    private void Unsubscribe()
    {
        if(_vehicleParams == null)
            return;

        _vehicleParams.RepairChanged -= OnRepairChanged;
        _vehicleParams.Destroyed -= OnVehicleDestroyed;
    }

    private void OnRepairChanged(float value)
    {
        if (value == 0)
            return;

        Destroy(gameObject);
    }

    private void OnVehicleDestroyed(VehicleParams vehicleParams) =>
        Destroy(gameObject);

    private void OnDestroy() =>
        Unsubscribe();
}