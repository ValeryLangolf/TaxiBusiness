#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.Rendering.CoreUtils;

public class RoadConnector
{
    private float _connectDistance;

    public void SetParams(float connectDistance) =>
        _connectDistance = connectDistance;

    public void Connect(List<SectionRoadStrip> sections)
    {
        foreach (SectionRoadStrip section in sections)
            ConnectSection(section, sections);
    }

    private void ConnectSection(SectionRoadStrip section, List<SectionRoadStrip> sections)
    {
        List<SectionRoadStrip> connectedSections = new();
        IReadOnlyList<Transform> points = section.Points;

        if (points == null || points.Count == 0)
        {
            Debug.LogWarning($"В секции \"{section.gameObject.name}\" не найдено точек пути!", section);
            return;
        }

        Vector3 endPoint = points[^1].position;

        foreach (SectionRoadStrip otherSection in sections)
        {
            if (otherSection == section)
                continue;

            IReadOnlyList<Transform> otherSectionPoints = otherSection.Points;

            if (otherSectionPoints == null || otherSectionPoints.Count == 0)
                continue;

            Vector3 otherStart = otherSectionPoints[0].position;
            float distance = Vector3.Distance(endPoint, otherStart);

            if (distance <= _connectDistance)
                connectedSections.Add(otherSection);
        }

        if(Utils.AreListsEqual(section.ConnectedLanes, connectedSections))
            return;

        section.SetConnectedSections(connectedSections);
        EditorUtility.SetDirty(section);
    }
}

#endif