using System.Collections.Generic;
using UnityEngine;

public class SectionRoadStrip : MonoBehaviour
{
    [Header("Автозаполняемые списки")]
    [SerializeField] private List<Waypoint> _points = new();
    [SerializeField] private List<SectionRoadStrip> _connectedSections = new();

    public List<Waypoint> Points => new(_points);

    public List<SectionRoadStrip> ConnectedSections => new(_connectedSections);

    public void SetPoints(List<Waypoint> points) =>
        _points = new(points);

    public void SetConnectedSections(List<SectionRoadStrip> connectedSections) =>
        _connectedSections = new(connectedSections);
}