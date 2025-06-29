using UnityEngine;

public class PlayerDispatcher : MonoBehaviour
{
    [SerializeField] private DestinationPointSpawner _pointSpawner;
    [SerializeField] private VehicleSelector _selector;
    [SerializeField] private MouseHitInformer _clickInformer;

    private void OnEnable()
    {
        _clickInformer.PlaneLeftClicked += OnPlaneClicked;
        _clickInformer.PassengerClicked += OnClickPassenger;
    }

    private void OnDisable()
    {
        _clickInformer.PlaneLeftClicked -= OnPlaneClicked;
        _clickInformer.PassengerClicked -= OnClickPassenger;
    }

    private void OnPlaneClicked(Vector3 position)
    {
        HandleClick(position, null);
        _pointSpawner.Spawn(position);
    }

    private void OnClickPassenger(Passenger passenger) =>
        HandleClick(passenger.Target.position, passenger);

    private void HandleClick(Vector3 position, Passenger passenger)
    {
        Vehicle vehicle = _selector.Vehicle;

        if (vehicle == null)
            return;

        if (vehicle.IsPassengerInCar)
            return;

        vehicle.SetPassenger(passenger);
        vehicle.SetDestination(position);

        SfxPlayer.Instance.PlayEngineRoar();
    }
}