using UnityEngine;

public class BottomPanel : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private FillButton _fuelFillButton;
    [SerializeField] private FillButton _repairFillButton;
    [SerializeField] private VehicleSelector _selector;
    [SerializeField] private BottomPanelView _view;
    [SerializeField] private BottomPanelShower _shower;
    [SerializeField] private DriverCard _driverManCard;
    [SerializeField] private DriverManSpawner _driverManSpaner;

    private void Start()
    {
        if (_selector.Vehicle != null)
            _shower.Show();
        else
            _shower.Hide();
    }

    private void Update()
    {
        Vehicle vehicle = _selector.Vehicle;

        if (vehicle != null)
        {
            Fill(vehicle.Params);
            return;
        }

        _fuelFillButton.StopSound();
        _repairFillButton.StopSound();
    }

    private void OnEnable()
    {
        _selector.Selected += OnSelected;
        _selector.Deselected += OnDeselected;

        _fuelFillButton.Pressed += OnFuelPressed;
        _fuelFillButton.Unpressed += OnFuelUnpressed;

        _repairFillButton.Pressed += OnRepairPressed;
        _repairFillButton.Unpressed += OnRepairUnpressed;

        _driverManCard.Clicked += OnClickDriverManCard;
    }

    private void OnDisable()
    {
        _selector.Selected -= OnSelected;
        _selector.Deselected -= OnDeselected;

        _fuelFillButton.Pressed -= OnFuelPressed;
        _fuelFillButton.Unpressed -= OnFuelUnpressed;

        _repairFillButton.Pressed -= OnRepairPressed;
        _repairFillButton.Unpressed -= OnRepairUnpressed;

        _driverManCard.Clicked -= OnClickDriverManCard;
    }

    private void Fill(VehicleParams vehicleParams)
    {
        if (vehicleParams.RemainingFuel < Constants.FullFuel)
            FillFuel(vehicleParams);

        if (vehicleParams.RemainingRepair < Constants.FullRepair)
            FillRepair(vehicleParams);
    }

    private void FillFuel(VehicleParams vehicleParams)
    {
        if (_fuelFillButton.IsPressed && vehicleParams.TryFillFuel(Constants.FuelFillingSpeed * Time.deltaTime))
        {
            _wallet.SpendOnEmptyWallet(FuelParams.CurrentPrice * Time.deltaTime);

            if (_fuelFillButton.IsPlaying == false)
                _fuelFillButton.PlaySound();

            return;
        }

        _fuelFillButton.StopSound();
    }

    private void FillRepair(VehicleParams vehicleParams)
    {
        if (_repairFillButton.IsPressed && vehicleParams.TryFillRepair(Constants.RepairFillingSpeed * Time.deltaTime))
        {
            _wallet.SpendOnEmptyWallet(RepairParams.CurrentPrice * Time.deltaTime);

            if (_repairFillButton.IsPlaying == false)
                _repairFillButton.PlaySound();

            return;
        }

        _repairFillButton.StopSound();
    }

    private void OnSelected(Vehicle vehicle)
    {
        VehicleParams vehicleParams = vehicle.Params;
        _view.SetSprite(vehicleParams.Sprite);
        OnFuelChanged(vehicleParams.RemainingFuel);
        OnRepairChanged(vehicleParams.RemainingRepair);

        if(vehicle.DriverMan != null)
            _driverManCard.SetSprite(vehicle.DriverMan.Sprite);

        SubscribeVehicle(vehicle);
        _shower.Show();
    }

    private void OnDeselected(Vehicle vehicle)
    {
        _view.ResetSprite();
        UnsubscribeVehicle(vehicle);
        _driverManCard.ResetSprite();
        _shower.Hide();
    }

    private void SubscribeVehicle(Vehicle vehicle)
    {
        vehicle.Params.FuelChanged += OnFuelChanged;
        vehicle.Params.RepairChanged += OnRepairChanged;
    }

    private void UnsubscribeVehicle(Vehicle vehicle)
    {
        vehicle.Params.FuelChanged -= OnFuelChanged;
        vehicle.Params.RepairChanged -= OnRepairChanged;
    }

    private void OnFuelChanged(float remainingFuel) =>
        _view.SetRemainingFuel(remainingFuel);

    private void OnRepairChanged(float remainingRepair) =>
        _view.SetRemainingRepair(remainingRepair);

    private void OnFuelPressed()
    {
        if (_selector.Vehicle.Params.RemainingFuel < Constants.FullFuel)
            _fuelFillButton.PlaySound();
    }

    private void OnFuelUnpressed() =>
        _fuelFillButton.StopSound();

    private void OnRepairPressed()
    {
        if (_selector.Vehicle.Params.RemainingRepair < Constants.FullRepair)
            _repairFillButton.PlaySound();
    }

    private void OnRepairUnpressed() =>
        _repairFillButton.StopSound();

    private void OnClickDriverManCard()
    {
        Vehicle vehicle = _selector.Vehicle;

        if (vehicle == null)
            return;

        if (vehicle.DriverMan != null)
        {
            vehicle.ResetDriverMan();
            _driverManCard.ResetSprite();
        }
        else
        {
            vehicle.SetDriverMan(_driverManSpaner.Spawn());
            _driverManCard.SetSprite(vehicle.DriverMan.Sprite);
        }
    }
}