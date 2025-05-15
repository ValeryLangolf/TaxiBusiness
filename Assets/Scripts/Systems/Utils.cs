using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utils
{
    public static float CalculateDistance(Vector3 pointA, Vector3 pointB) =>
        Vector3.Distance(pointA, pointB);

    public static float CalculateDistance(SectionRoadStrip laneA, SectionRoadStrip laneB) =>
        Vector3.Distance(laneA.Points.Last().Position, laneB.Points.Last().Position);

    public static bool AreListsEqual<T>(List<T> list1, List<T> list2) where T : Component
    {
        if (list1 == null && list2 == null) return true;
        if (list1 == null || list2 == null) return false;
        if (list1.Count != list2.Count) return false;

        var sortedList1 = list1.OrderBy(item => item.name).ToList();
        var sortedList2 = list2.OrderBy(item => item.name).ToList();

        return sortedList1.SequenceEqual(sortedList2);
    }

    public static Waypoint GetNearestSectionAndPoint(Vector3 position, List<SectionRoadStrip> sections)
    {
        Waypoint waypoint = null;
        float minDistance = Mathf.Infinity;

        foreach (SectionRoadStrip section in sections)
        {
            Waypoint point = section.GetClosestPoint(position);
            float distance = Vector3.Distance(position, point.Position);

            if (distance < minDistance)
            {
                minDistance = distance;
                waypoint = point;
            }
        }

        return waypoint;
    }

    public static (string, int) ExtractName(string name)
    {
        Match match = Regex.Match(name, @"(\D+)(\d*)");
        string textPart = match.Groups[1].Value;
        int numberPart = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;

        return (textPart, numberPart);
    }
}