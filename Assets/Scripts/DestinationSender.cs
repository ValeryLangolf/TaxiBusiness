using UnityEngine;

public class DestinationSender : MonoBehaviour
{
    [SerializeField] private DestinationMarker _marker;
    [SerializeField] private VehicleSelector _selector;

    private void Awake() =>
        _marker.gameObject.SetActive(false);

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            HandleClick();
    }

    private void OnEnable() =>
        _selector.VehicleSelectChanged += HandleVehicleSelectChanged;

    private void OnDisable() =>
        _selector.VehicleSelectChanged -= HandleVehicleSelectChanged;

    private void HandleVehicleSelectChanged(VehicleController vehicle)
    {
        if (vehicle == null)
            _marker.gameObject.SetActive(false);
        else
            _marker.gameObject.SetActive(true);
    }

    private void HandleClick()
    {
        VehicleController vehicle = _selector.Vehicle; 
        
        if (vehicle == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.TryGetComponent(out Plane _))
            {
                _marker.transform.position = hit.point;
                _marker.gameObject.SetActive(true);
            }
        }
    }
}