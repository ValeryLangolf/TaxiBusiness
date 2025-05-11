#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class SectionRoadBuilder : MonoBehaviour
{
    private const string Waypoint = nameof(Waypoint);
    private const int MinWaypointsCount = 2;

    [Header("Создаёт конкретное количество точек")]
    [SerializeField, Range(MinWaypointsCount, 100)] private int _count = 2;

    [Header("Если количество точек зависит от расстояния:")]
    [SerializeField] private bool _isDistanceDependency;
    [SerializeField, Range(0.05f, 100)] private float _distance = 1f;

    [Header("Изгиб линии:")]
    [SerializeField] private Vector3 _arcValue = new();
    //[SerializeField, Range(0f, 5f)] private float _curvatureFactor = 1.0f;

    private void OnValidate()
    {
        List<Transform> transforms = new();

        if (_isDistanceDependency || _arcValue != Vector3.zero)
        {
            transforms = GetChildren();
            transforms = GetSortedByName(transforms);
        }

        if (_isDistanceDependency)
        {
            _count = (int)(Vector3.Distance(transforms.First().position, transforms.Last().position) / _distance);

            if (_count < MinWaypointsCount)
                _count = MinWaypointsCount;
        }

        if(_arcValue != Vector3.zero)
        {
            EqualizeDistanceBetweenPoints(transforms);
        }
    }

    public void UpdateCount()
    {
        List<Transform> transforms = GetChildren();
        transforms = GetSortedByName(transforms);

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
            newElement.transform.SetParent(transform);
            int secondLastIndex = transforms.Count - 1;
            transforms.Insert(secondLastIndex, newElement.transform);
        }

        RenameChildren(transforms);
        SortChildren(transforms);
        EqualizeDistanceBetweenPoints(transforms);
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

    private void EqualizeDistanceBetweenPoints(List<Transform> transforms)
    {
        if (transforms.Count <= MinWaypointsCount)
            return;

        Vector3 firstPosition = transforms.First().position;
        Vector3 lastPosition = transforms.Last().position;

        float totalDistance = Vector3.Distance(firstPosition, lastPosition);
        float smoothFactor = Mathf.Clamp(totalDistance * 0.05f, 0.5f, 2.5f);
        Vector3 scaledArc = _arcValue * smoothFactor;

        for (int i = 1; i < transforms.Count - 1; i++)
        {
            float t = (float)i / (transforms.Count - 1);
            Vector3 pointOnLine = Vector3.Lerp(firstPosition, lastPosition, t);

            float curveFactor = Mathf.Sin(t * Mathf.PI);
            Vector3 arcOffset = scaledArc * curveFactor;

            transforms[i].position = pointOnLine + arcOffset;
        }
    }
}
#endif