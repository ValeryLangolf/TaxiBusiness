using UnityEngine;

public class VehicleCollider : MonoBehaviour
{
    private Vehicle _vehicle;

    private void Awake() =>
        _vehicle = GetComponentInParent<Vehicle>();

    public Vehicle Vehicle => _vehicle;
}