#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class SectionRoadBuilder : MonoBehaviour
{
    private const string Waypoint = nameof(Waypoint);

    [SerializeField, Range(2, 20)] private int _count = 2;

    public void UpdateCount()
    {
        List<Transform> transforms = GetComponentsInChildren<Transform>(true).Where(t => t != transform).ToList();
        transforms = OrderByName(transforms);

        if (transforms.Count < 2)
        {
            Debug.LogWarning("ƒолжно быть как минимум 2 точки дл€ обновлени€");
            return;
        }

        Debug.Log($"{transforms.Count}");

        Transform firstPoint = transforms.First();
        transforms.Remove(firstPoint);
        firstPoint.gameObject.name = $"{Waypoint}0";

        Transform lastPoint = transforms.Last();
        transforms.Remove(lastPoint);
        lastPoint.gameObject.name = $"{Waypoint}{_count - 1}";

        Debug.Log($"{transforms.Count}");

        int localCount = _count - 2;
        int rightShift = 1;

        for (int i = 0; i < transforms.Count; i++)
            transforms[i].name = $"{Waypoint}{i + rightShift}";

        while (transforms.Count > localCount)
        {
            DestroyImmediate(transforms[^1].gameObject);
            transforms.RemoveAt(transforms.Count - 1);
        }

        for (int i = transforms.Count + 1; i < localCount + rightShift; i++)
        {
            GameObject newWaypoint = new($"{Waypoint}{i}");
            newWaypoint.transform.SetParent(transform);
            transforms.Add(newWaypoint.transform);
        }

        for (int i = 1; i < transforms.Count + 1; i++)
        {
            transforms[i - 1].position = Vector3.Lerp(
                firstPoint.position,
                lastPoint.position,
                (float)i / (transforms.Count + rightShift));
        }

        SortChildren();
    }

    private List<Transform> OrderByName(List<Transform> transforms) =>
        transforms.OrderBy(t => ExtractName(t.name)).ToList();

    private void SortChildren()
    {
        var children = GetComponentsInChildren<Transform>(true)
            .Where(t => t != transform)
            .ToList();

        var sortedChildren = children.OrderBy(t => ExtractName(t.name)).ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
            sortedChildren[i].SetSiblingIndex(i);
    }

    private (string, int) ExtractName(string name)
    {
        var match = Regex.Match(name, @"(\D+)(\d*)");
        string textPart = match.Groups[1].Value;
        int numberPart = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;

        return (textPart, numberPart);
    }
}

#endif