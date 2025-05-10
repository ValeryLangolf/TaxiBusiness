using UnityEngine;

public class VehiclePointer : MonoBehaviour
{
    [SerializeField] float _speedRotation;

    private void Update() =>
        transform.localRotation *= Quaternion.Euler(0, 0, _speedRotation * Time.deltaTime);
}