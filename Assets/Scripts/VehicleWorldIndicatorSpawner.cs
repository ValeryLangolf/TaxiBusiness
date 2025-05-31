using UnityEngine;

public class VehicleWorldIndicatorSpawner : MonoBehaviour
{
    [SerializeField] private LackFuelIndicator _lackFuelIndicatorPrefab;
    [SerializeField] private LackRepairIndicator _lackRepairIndicatorPrefab;

    public void SpawnFuelIndicator(VehicleParams vehicleParams)
    {
        LackFuelIndicator indicator = Instantiate(_lackFuelIndicatorPrefab, transform);
        indicator.SetVehicle(vehicleParams);
    }

    public void SpawnRepairIndicator(VehicleParams vehicleParams)
    {
        LackRepairIndicator indicator = Instantiate(_lackRepairIndicatorPrefab, transform);
        indicator.SetVehicle(vehicleParams);
    }
}