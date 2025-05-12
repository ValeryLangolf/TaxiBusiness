using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DestinationSender : MonoBehaviour
{
    [SerializeField] private RoadNetwork _roadNetwork;
    [SerializeField] private Pathfinder _pathfinder;
    [SerializeField] private DestinationMarker _marker;
    [SerializeField] private VehicleSelector _selector;

    private bool _isRouteAssigned;

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
        else if (_isRouteAssigned)
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
                _marker.SetPosition(hit.point);
                _marker.gameObject.SetActive(true);

                SendToDestination(vehicle, hit.point);
            }
        }
    }

    private void SendToDestination(VehicleController vehicle, Vector3 destination)
    {
        PointInRoadSection start = Utils.GetNearestSectionAndPoint(vehicle.transform.position, _roadNetwork.Sections);
        PointInRoadSection end = Utils.GetNearestSectionAndPoint(destination, _roadNetwork.Sections);

        if (start == null)
            Debug.LogWarning("Не удалось получить стартовую позицию транспортного средства в пределах дорожной сети");
        
        if (end == null)
            Debug.LogWarning("Не удалось получить пункт назначения транспортного средства в пределах дорожной сети");

        List<Transform> path = _pathfinder.FindPath(start, end);

        if (path.Count == 0)
            Debug.LogWarning("Не удалось найти путь");

        _isRouteAssigned = true;
        vehicle.SetPath(path);
        Debug.Log("Тачка выехала");
    }
}
