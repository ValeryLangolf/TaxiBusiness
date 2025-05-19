public class VehicleParams
{
    private Vehicle _vehicle;
    private VehicleCard _card;

    public VehicleParams(Vehicle vehicle, VehicleCard card)
    {
        _vehicle = vehicle;
        _card = card;
    }

    public Vehicle Vehicle => _vehicle;

    public VehicleCard Card => _card;
}