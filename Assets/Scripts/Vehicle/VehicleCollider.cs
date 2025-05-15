using UnityEngine;

public class VehicleCollider : MonoBehaviour
{
    [SerializeField] private Vehicle _vehicle;

    public Vehicle Vehicle => _vehicle;
}