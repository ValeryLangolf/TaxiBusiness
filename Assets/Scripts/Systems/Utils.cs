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

        var filteredList1 = list1.Where(item => item != null).OrderBy(item => item.name).ToList();
        var filteredList2 = list2.Where(item => item != null).OrderBy(item => item.name).ToList();

        return filteredList1.SequenceEqual(filteredList2);
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
        if (string.IsNullOrEmpty(name))
            return (string.Empty, 0);

        Match match = Regex.Match(name, @"(\D+)(\d*)");

        if (match.Success == false)
            return (string.Empty, 0);

        string textPart = match.Groups[1].Value;

        int numberPart = 0;

        if (match.Groups[2].Success && !string.IsNullOrEmpty(match.Groups[2].Value))
            if (!int.TryParse(match.Groups[2].Value, out numberPart))
                numberPart = 0;

        return (textPart, numberPart);
    }

    public static float CalculateDistancePath(List<Waypoint> path)
    {
        float distance = 0;

        foreach (Waypoint waypoint in path)
            distance += waypoint.LenghtInMeter;

        return distance;
    }

    public static bool HasChanges<T>(IEnumerable<T> newCollection, IEnumerable<T> oldCollection)
    {
        if (newCollection == null || oldCollection == null)
            return true;

        if (ReferenceEquals(newCollection, oldCollection))
            return false;

        if (newCollection.Count() != oldCollection.Count())
            return true;

        HashSet<T> newSet = new HashSet<T>(newCollection);
        return oldCollection.Any(item => newSet.Contains(item) == false);
    }

    public static bool HasChanges<T>(IEnumerable<T> newCollection, IEnumerable<T> oldCollection, IEqualityComparer<T> comparer)
    {
        if (newCollection == null || oldCollection == null)
            return true;

        if (ReferenceEquals(newCollection, oldCollection))
            return false;

        if (newCollection.Count() != oldCollection.Count())
            return true;

        HashSet<T> newSet = new HashSet<T>(newCollection, comparer);
        return oldCollection.Any(item => newSet.Contains(item) == false);
    }


}