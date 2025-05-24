public class VehicleParams
{
    private readonly Vehicle _vehicle;
    private readonly VehicleIcon _card;

    public VehicleParams(Vehicle vehicle, VehicleIcon card)
    {
        _vehicle = vehicle;
        _card = card;
    }

    public Vehicle Vehicle => _vehicle;

    public VehicleIcon Card => _card;
}