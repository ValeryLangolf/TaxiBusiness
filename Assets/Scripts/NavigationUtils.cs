using System.Linq;
using UnityEngine;

public static class NavigationUtils
{
    public static float CalculateDistance(SectionRoadStrip laneA, SectionRoadStrip laneB) =>
        Vector3.Distance(laneA.Points.Last().position, laneB.Points.Last().position);
}