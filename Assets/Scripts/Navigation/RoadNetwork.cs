using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    [Header("Автозаполняемый список")]
    [SerializeField] private List<SectionRoadStrip> _sections;

    private readonly List<Waypoint> _points = new();

    public static RoadNetwork Instance { get; private set; }

    public List<SectionRoadStrip> Sections => new(_sections);

    public List<Waypoint> Points => new(_points);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ReconstructByPoints();
    }

    public void SetSections(List<SectionRoadStrip> sections) =>
        _sections = new(sections);

    private void ReconstructByPoints()
    {
        AddPointsToList();

        foreach (SectionRoadStrip section in _sections)
        {
            List<Waypoint> waypoints = section.Points;

            for (int j = 0; j < waypoints.Count; j++)
            {
                if (j + 1 < waypoints.Count)
                    waypoints[j].AddConnectedPoint(waypoints[j + 1]);

                if (j == waypoints.Count - 1)
                    ConnectLastWaypointToConnectedSections(waypoints[j], section.ConnectedSections);
            }
        }
    }

    private void ConnectLastWaypointToConnectedSections(Waypoint lastWaypoint, List<SectionRoadStrip> connectedSections)
    {
        foreach (SectionRoadStrip connectedSection in connectedSections)
        {
            Waypoint firstWaypoint = connectedSection.Points.First();
            lastWaypoint.AddConnectedPoint(firstWaypoint);
        }
    }

    private void AddPointsToList()
    {
        foreach (SectionRoadStrip section in _sections)
            foreach (Waypoint waypoint in section.Points)
                _points.Add(waypoint);
    }
}