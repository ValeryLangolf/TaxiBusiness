using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDispatcher : MonoBehaviour
{
    private readonly List<Transform> _path;

    public static event Action<Vector3> PlaneClicked;

    private void OnEnable() =>
        MouseHitInformer.LeftHitted += HandleHitted;

    private void OnDisable() =>
        MouseHitInformer.LeftHitted -= HandleHitted;

    private void HandleHitted(Collider collider, Vector3 hitPoint)
    {
        Vehicle vehicle = VehicleSelector.Vehicle;

        if (vehicle == null)
            return;

        if (collider.TryGetComponent(out Plane _) == false)
            return;

        PlaneClicked?.Invoke(hitPoint);
        SendToDestination(vehicle, hitPoint);
    }

    private void SendToDestination(Vehicle vehicle, Vector3 destination)
    {
        Waypoint start = Utils.GetNearestSectionAndPoint(vehicle.transform.position, RoadNetwork.Instance.Sections);
        Waypoint end = Utils.GetNearestSectionAndPoint(destination, RoadNetwork.Instance.Sections);

        if (start == null)
            throw new ArgumentNullException("Ќе удалось получить стартовую позицию транспортного средства в пределах дорожной сети");

        if (end == null)
            throw new ArgumentNullException("Ќе удалось получить пункт назначени€ транспортного средства в пределах дорожной сети");

        Debug.Log($"{start.Section.name} - {start.name}; {end.Section.name} - {end.name}");

        List<SectionRoadStrip> sections = Pathfinder.FindPath(start, end);

        if (sections == null || sections.Count == 0)
            throw new ArgumentNullException("—писок секций пути пуст!");

        sections.Remove(start.Section);
        sections.Remove(end.Section);

        List<Waypoint> path = AddPathFistSection(start);
        Debug.Log("List<Waypoint> path = AddPathFistSection(start);");

        path.AddRange(GetPointPath(sections));
        Debug.Log("path.AddRange(GetPointPath(sections));");

        path.AddRange(AddPathLastSection(end));
        Debug.Log("path.AddRange(AddPathLastSection(end))");

        if (path.Count == 0)
            Debug.LogWarning("Ќе удалось найти путь");

        vehicle.SetPath(path);
        Debug.Log("“ачка выехала");
    }

    private List<Waypoint> GetPointPath(List<SectionRoadStrip> sections)
    {
        List<Waypoint> path = new();

        if (sections.Count == 0)
            return path;

        foreach (SectionRoadStrip section in sections)
            foreach (Waypoint point in section.Points)
                path.Add(point);

        if (path.Count == 0)
            throw new ArgumentNullException("—писок точек пути пуст!");

        return path;
    }

    private List<Waypoint> AddPathFistSection(Waypoint start)
    {
        List<Waypoint> paths = new(start.Section.Points);

        while (start != paths[0])
            paths.RemoveAt(0);

        return paths;
    }

    private List<Waypoint> AddPathLastSection(Waypoint last)
    {
        List<Waypoint> path = new(last.Section.Points);

        while (last != path[^1])
            path.RemoveAt(path.Count - 1);

        return path;
    }
}