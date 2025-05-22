public class VehicleParams
{
    private Vehicle _vehicle;
    private VehicleIcon _card;

    public VehicleParams(Vehicle vehicle, VehicleIcon card)
    {
        _vehicle = vehicle;
        _card = card;
    }

    public Vehicle Vehicle => _vehicle;

    public VehicleIcon Card => _card;
}