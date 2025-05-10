using UnityEngine;

public class VehicleCollider : MonoBehaviour
{
    [SerializeField] private VehicleController _vehicle;

    public VehicleController VehicleController => _vehicle;
}