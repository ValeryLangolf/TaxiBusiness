#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoadConnector
{
    public void Connect(List<SectionRoadStrip> sections, float connectDistance)
    {
        foreach (SectionRoadStrip section in sections)
            ConnectSection(section, sections, connectDistance);
    }

    private void ConnectSection(SectionRoadStrip section, List<SectionRoadStrip> sections, float connectDistance)
    {
        List<SectionRoadStrip> connectedSections = new();
        List<Waypoint> points = new(section.Points);

        if (points == null || points.Count == 0)
        {
            Debug.LogWarning($"В секции \"{section.gameObject.name}\" не найдено точек пути!", section);
            return;
        }

        Vector3 endPoint = points[^1].Position;

        foreach (SectionRoadStrip otherSection in sections)
        {
            if (otherSection == section)
                continue;

            List<Waypoint> otherSectionPoints = new(otherSection.Points);

            if (otherSectionPoints == null || otherSectionPoints.Count == 0)
                continue;

            Vector3 otherStart = otherSectionPoints[0].Position;
            float distance = Vector3.Distance(endPoint, otherStart);

            if (distance <= connectDistance)
                connectedSections.Add(otherSection);
        }

        if(Utils.AreListsEqual(section.ConnectedSections, connectedSections))
            return;

        section.SetConnectedSections(connectedSections);
        EditorUtility.SetDirty(section);
    }
}
#endif