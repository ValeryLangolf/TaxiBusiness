using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.CoreUtils;

public class DestinationSender : MonoBehaviour
{
    [SerializeField] private RoadNetwork _roadNetwork;
    [SerializeField] private Pathfinder _pathfinder;
    [SerializeField] private DestinationMarker _marker;
    [SerializeField] private VehicleSelector _selector;
    [SerializeField] private IconFollower _iconStart;
    [SerializeField] private IconFollower _iconFinish;

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

        _iconStart.Follow(start.Point);
        _iconFinish.Follow(end.Point);

        Debug.Log($"{start.Section.name} - {start.Point.name}; {end.Section.name} - {end.Point.name}");

        Debug.Log("List<Transform> path");

        List<SectionRoadStrip> sections = _pathfinder.FindPath(start, end);
        sections.RemoveAt(0);
        sections.RemoveAt(sections.Count - 1);

        List<Transform> path = AddPathFistSection(start);
        path.AddRange(GetPointPath(sections));
        path.AddRange(AddPathLastSection(end));

        if (path.Count == 0)
            Debug.LogWarning("Не удалось найти путь");

        _isRouteAssigned = true;
        vehicle.SetPath(path);
        Debug.Log("Тачка выехала");
    }

    private List<Transform> GetPointPath(List<SectionRoadStrip> sections)
    {
        Debug.Log($"sections.Count = {sections.Count}");

        List<Transform> path = new();

        foreach (SectionRoadStrip section in sections)
            foreach (Transform point in section.Points)
                path.Add(point);

        Debug.Log($"Точек: {path.Count}");
        Debug.Log($"Первая: {path[0].name}, секция: {sections[0].name}");
        return path;
    }

    private List<Transform> AddPathFistSection(PointInRoadSection start)
    {
        List<Transform> path = new(start.Section.Points);

        while (start.Point != path[0])
            path.RemoveAt(0);

        return path;
    }

    private List<Transform> AddPathLastSection(PointInRoadSection last)
    {
        List<Transform> path = new(last.Section.Points);

        while (last.Point != path[^1])
            path.RemoveAt(path.Count - 1);

        return path;
    }
}