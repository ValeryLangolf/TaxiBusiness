using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class Drower
{
    public void DrowSections(List<SectionRoadStrip> sections, Color color)
    {
        Gizmos.color = color;

        foreach (SectionRoadStrip section in sections)
            DrowSection(section);
    }

    public void DrowSection(SectionRoadStrip section)
    {
        if (section.Points == null || section.Points.Count < 2)
            return;
        
        IReadOnlyList<Transform> points = section.Points;

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (section.Points[i] == null || points[i + 1] == null)
                continue;

            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
    }
}
#endif