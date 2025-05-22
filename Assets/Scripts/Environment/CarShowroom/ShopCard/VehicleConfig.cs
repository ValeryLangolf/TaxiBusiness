using UnityEngine;

[CreateAssetMenu(fileName = "NewVehicle", menuName = "Vehicles/New Vehicle Data")]
public class VehicleConfig : ScriptableObject
{
    public Vehicle Prefab;
    public string Name;
    public Sprite CarImage;
    public float Speed;
    public float Strength;
    public float Petrol;
    public float MoneyRate;
    public int Price;
}