using UnityEngine;

public class LackFuelIndicator : MonoBehaviour
{
    [SerializeField] private UiFollower _follower;

    private VehicleParams _vehicleParams;

    public void SetVehicle(VehicleParams vehicleParams)
    {
        if(vehicleParams == null)
            Destroy(gameObject);

        _vehicleParams = vehicleParams;
        _follower.Follow(vehicleParams.transform);
        Subscribe();
    }

    private void Subscribe()
    {
        _vehicleParams.FuelChanged += OnFuelChanged;
        _vehicleParams.Destroyed += OnVehicleDestroyed;
    }

    private void Unsubscribe()
    {
        if(_vehicleParams == null)
            return;

        _vehicleParams.FuelChanged -= OnFuelChanged;
        _vehicleParams.Destroyed -= OnVehicleDestroyed;
    }

    private void OnFuelChanged(float value)
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