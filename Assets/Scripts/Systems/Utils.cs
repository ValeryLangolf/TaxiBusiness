using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static float CalculateDistance(SectionRoadStrip laneA, SectionRoadStrip laneB) =>
        Vector3.Distance(laneA.Points.Last().position, laneB.Points.Last().position);

    public static bool AreListsEqual<T>(List<T> list1, List<T> list2) where T : Component
    {
        if (list1 == null && list2 == null) return true;
        if (list1 == null || list2 == null) return false;
        if (list1.Count != list2.Count) return false;

        var sortedList1 = list1.OrderBy(item => item.GetInstanceID()).ToList();
        var sortedList2 = list2.OrderBy(item => item.GetInstanceID()).ToList();

        return sortedList1.SequenceEqual(sortedList2);
    }

    public static (SectionRoadStrip, Transform) GetNearestSectionAndPoint(Vector3 position, List<SectionRoadStrip> sections)
    {
        SectionRoadStrip nearestLane = null;
        Transform nearestPoint = null;
        float minDistance = Mathf.Infinity;

        foreach (SectionRoadStrip lane in sections)
        {
            Transform point = lane.GetClosestPoint(position);
            float distance = Vector3.Distance(position, point.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestLane = lane;
                nearestPoint = point;
            }
        }

        return (nearestLane, nearestPoint);
    }

    public static List<Vector3> GetPathBetweenPoints(
        List<SectionRoadStrip> lanePath,
        SectionRoadStrip startLane,
        Transform startPoint,
        SectionRoadStrip endLane,
        Transform endPoint)
    {
        List<Vector3> path = new();

        int startIndex = startLane.GetPointIndex(startPoint);

        for (int i = startIndex; i < startLane.Points.Count; i++)
            path.Add(startLane.Points[i].position);

        for (int i = 1; i < lanePath.Count - 1; i++)
            path.AddRange(lanePath[i].Points.Select(p => p.position));

        int endIndex = endLane.GetPointIndex(endPoint);

        for (int i = 0; i <= endIndex; i++)
            path.Add(endLane.Points[i].position);

        return path;
    }
}