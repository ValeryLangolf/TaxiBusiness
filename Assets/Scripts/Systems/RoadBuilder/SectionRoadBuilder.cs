#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SectionRoadBuilder : MonoBehaviour
{
    private const string Waypoint = nameof(Waypoint);
    public const int MinWaypointsCount = 2;

    [Header("Создаёт конкретное количество точек")]
    [SerializeField, Range(MinWaypointsCount, 100)] private int _count = 2;

    [Header("Если количество точек зависит от расстояния:")]
    [SerializeField] private bool _isDistanceDependency;
    [SerializeField, Range(0.05f, 100)] private float _distance = 1f;

    [Header("Изгиб линии:")]
    [SerializeField] private bool _isAutoEqualizingPoint = true;
    [SerializeField] private Vector3 _arcValue = new();

    private readonly PointEqualizer _pointEqualizer = new();

    public void UpdateCount()
    {
        List<Transform> transforms = GetChildren();
        transforms = GetSortedByName(transforms);

        if (_isDistanceDependency)
            _count = (int)(Vector3.Distance(transforms.First().position, transforms.Last().position) / _distance);

        if (_count < MinWaypointsCount)
            _count = MinWaypointsCount;

        if (transforms.Count < MinWaypointsCount)
        {
            RenameChildren(transforms);
            SortChildren(transforms);
            Debug.LogWarning("Должно заранее существовать как минимум 2 точки");

            return;
        }

        while (transforms.Count > _count)
        {
            Transform secondLastElement = transforms[^2];
            DestroyImmediate(secondLastElement.gameObject);
            transforms.Remove(secondLastElement);
        }

        while (transforms.Count < _count)
        {
            GameObject newElement = new($"{Waypoint}");
            newElement.AddComponent<Waypoint>();
            newElement.transform.SetParent(transform);
            int secondLastIndex = transforms.Count - 1;
            transforms.Insert(secondLastIndex, newElement.transform);
        }

        RenameChildren(transforms);
        SortChildren(transforms);

        if (_isAutoEqualizingPoint)
            _pointEqualizer.EqualizeDistanceBetweenPoints(transforms, _arcValue);
    }

    private List<Transform> GetChildren()
    {
        return GetComponentsInChildren<Transform>(true)
            .Where(t => t != transform)
            .ToList();
    }

    private void RenameChildren(List<Transform> transforms)
    {
        for (int i = 0; i < transforms.Count; i++)
            transforms[i].name = $"{Waypoint}{i}";
    }

    private List<Transform> GetSortedByName(List<Transform> transforms) =>
        transforms.OrderBy(t => Utils.ExtractName(t.name)).ToList();

    private void SortChildren(List<Transform> transforms)
    {
        for (int i = 0; i < transforms.Count; i++)
            transforms[i].SetSiblingIndex(i);
    }
}
#endif