#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GizmosRoadDrower
{
    private Color _sectionColor;
    private Color _connectionColor;
    private float _waypointSphereRadius;
    private float _connectionSphereRadius;
    private List<SectionRoadStrip> _sections = new();

    public void SetParams(Color sectionColor, Color connectionColor, float waypointSphereRadius, float connectionSphereRadius)
    {
        _sectionColor = sectionColor;
        _connectionColor = connectionColor;
        _waypointSphereRadius = waypointSphereRadius;
        _connectionSphereRadius = connectionSphereRadius;

        if (_waypointSphereRadius < 0.05f)
            Debug.LogWarning($"Диаметр рисуемых сфер ({_waypointSphereRadius}м) подозрительно мал");
    }

    public void Drow(List<SectionRoadStrip> sections)
    {
        _sections = new(sections);

        if (_sections == null || _sections.Count == 0)
        {
            Debug.LogWarning($"Нет доступных секций для отрисовки");
            return;
        }

        DrowSections();
        DrowSpheres();
        DrowLinesConnection();
        DrowSpheresConnection();
    }

    private void DrowSections()
    {
        Gizmos.color = _sectionColor;

        foreach (SectionRoadStrip section in _sections)
            DrowSection(section);
    }

    private void DrowSection(SectionRoadStrip section)
    {
        if (section.Points == null || section.Points.Count < 2)
            return;

        IReadOnlyList<Waypoint> points = section.Points;

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (section.Points[i] == null || points[i + 1] == null)
                continue;

            Gizmos.DrawLine(points[i].Position, points[i + 1].Position);
        }
    }

    private void DrowSpheres()
    {
        Gizmos.color = _sectionColor;        

        foreach (SectionRoadStrip section in _sections)
            DrowSphere(section);
    }

    private void DrowSphere(SectionRoadStrip section)
    {
        if (section == null)
        {
            Debug.LogWarning("Переданная секция в DrowSphere имеет нулевую ссылку");
            return;
        }

        IReadOnlyList<Waypoint> points = section.Points;

        if (points == null || points.Count == 0)
            return;

        for (int i = 0; i < points.Count; i++)
        {
            if (points[i] == null)
                continue;

            Gizmos.DrawSphere(points[i].Position, _waypointSphereRadius);
        }
    }

    private void DrowLinesConnection()
    {
        Gizmos.color = _connectionColor;

        foreach (SectionRoadStrip section in _sections)
            DrowLineConnection(section);
    }

    private void DrowLineConnection(SectionRoadStrip section)
    {
        IReadOnlyList<SectionRoadStrip> connectedLanes = section.ConnectedSections;

        if (connectedLanes == null || connectedLanes.Count == 0)
            return;

        IReadOnlyList<Waypoint> points = section.Points;

        if (points.Count == 0)
            return;

        foreach (SectionRoadStrip connectedLane in connectedLanes)
        {
            if (connectedLane == null)
                continue;

            if (connectedLane.Points.Count == 0)
                continue;

            Waypoint startPoint = points.Last();
            Waypoint endPoint = connectedLane.Points.First();

            Gizmos.DrawLine(startPoint.Position, endPoint.Position);
        }
    }

    private void DrowSpheresConnection()
    {
        Gizmos.color = _connectionColor;

        foreach (SectionRoadStrip section in _sections)
            DrowSphereConnection(section);
    }

    private void DrowSphereConnection(SectionRoadStrip section)
    {
        IReadOnlyList<SectionRoadStrip> connectedLanes = section.ConnectedSections;

        if (connectedLanes == null || connectedLanes.Count == 0)
            return;

        foreach (SectionRoadStrip connectedLane in connectedLanes)
        {
            if (connectedLane == null)
                continue;

            if (section.Points.Count == 0 || connectedLane.Points.Count == 0)
                continue;

            Waypoint startPoint = section.Points.Last();
            Waypoint endPoint = connectedLane.Points.First();

            Gizmos.DrawSphere(startPoint.Position, _connectionSphereRadius);
            Gizmos.DrawSphere(endPoint.Position, _connectionSphereRadius);
        }
    }
}

#endif