using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utils
{
    public static bool AreListsEqual<T>(List<T> list1, List<T> list2) where T : Component
    {
        if (list1 == null && list2 == null) 
            return true;

        if (list1 == null || list2 == null) 
            return false;

        if (list1.Count != list2.Count) 
            return false;

        var sortedList1 = list1.OrderBy(item => item.name).ToList();
        var sortedList2 = list2.OrderBy(item => item.name).ToList();

        return sortedList1.SequenceEqual(sortedList2);
    }

    public static Waypoint GetNearestSectionAndPoint(Vector3 position, List<Waypoint> waypoints)
    {
        Waypoint closestPoint = null;
        float minDistance = Mathf.Infinity;

        foreach (Waypoint waypoint in waypoints)
        {
            float distance = Vector3.Distance(position, waypoint.Position);

            if (distance >= minDistance)
                continue;

            minDistance = distance;
            closestPoint = waypoint;
        }

        return closestPoint;
    }

    public static (string, int) ExtractName(string name)
    {
        Match match = Regex.Match(name, @"(\D+)(\d*)");
        string textPart = match.Groups[1].Value;
        int numberPart = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;

        return (textPart, numberPart);
    }
}