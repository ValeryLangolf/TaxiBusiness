using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utils
{
    public static float CalculateDistance(Vector3 pointA, Vector3 pointB) =>
        Vector3.Distance(pointA, pointB);

    public static float CalculateDistance(SectionRoadStrip laneA, SectionRoadStrip laneB) =>
        Vector3.Distance(laneA.Points.Last().position, laneB.Points.Last().position);

    public static bool AreListsEqual<T>(List<T> list1, List<T> list2) where T : Component
    {
        if (list1 == null && list2 == null) return true;
        if (list1 == null || list2 == null) return false;
        if (list1.Count != list2.Count) return false;

        var sortedList1 = list1.OrderBy(item => item.name).ToList();
        var sortedList2 = list2.OrderBy(item => item.name).ToList();

        return sortedList1.SequenceEqual(sortedList2);
    }

    public static PointInRoadSection GetNearestSectionAndPoint(Vector3 position, List<SectionRoadStrip> sections)
    {
        PointInRoadSection pointInsideSection = null;
        float minDistance = Mathf.Infinity;

        foreach (SectionRoadStrip section in sections)
        {
            Transform point = section.GetClosestPoint(position);
            float distance = Vector3.Distance(position, point.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                pointInsideSection = new(section, point);
            }
        }

        return pointInsideSection;
    }

    public static List<Vector3> GetPathBetweenPoints(
        List<SectionRoadStrip> lanePath,
        PointInRoadSection start,
        PointInRoadSection end)
    {
        List<Vector3> path = new();

        int startIndex = start.Section.GetPointIndex(start.Point);

        for (int i = startIndex; i < start.Section.Points.Count; i++)
            path.Add(start.Section.Points[i].position);

        for (int i = 1; i < lanePath.Count - 1; i++)
            path.AddRange(lanePath[i].Points.Select(p => p.position));

        int endIndex = end.Section.GetPointIndex(end.Point);

        for (int i = 0; i <= endIndex; i++)
            path.Add(end.Section.Points[i].position);

        return path;
    }

    public static (string, int) ExtractName(string name)
    {
        Match match = Regex.Match(name, @"(\D+)(\d*)");
        string textPart = match.Groups[1].Value;
        int numberPart = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;

        return (textPart, numberPart);
    }
}