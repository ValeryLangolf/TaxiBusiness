using System;
using UnityEngine;

public class VehicleSelector : MonoBehaviour
{
    [SerializeField] private VehiclePointer _pointer;
    [SerializeField] private Vector3 _offset;

    private VehicleController _selectedVehicle;

    public event Action<VehicleController> VehicleSelectChanged;

    public VehicleController Vehicle => _selectedVehicle;

    private void Awake() =>
        _pointer.gameObject.SetActive(false);

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleClick();

        if (_selectedVehicle != null)
            Follow();
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.TryGetComponent(out VehicleCollider vehicleCollider))
            {
                _pointer.gameObject.SetActive(true);
                _selectedVehicle = vehicleCollider.VehicleController;
            }
            else
            {
                _pointer.gameObject.SetActive(false);
                _selectedVehicle = null;
            }

            VehicleSelectChanged?.Invoke(_selectedVehicle);
        }
    }

    private void Follow() =>
        _pointer.transform.position = _selectedVehicle.transform.position + _offset;
}