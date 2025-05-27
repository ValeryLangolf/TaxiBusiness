#if UNITY_EDITOR
using System.Linq;
#endif

using System.Collections.Generic;
using UnityEngine;

public class SectionRoadStrip : MonoBehaviour
{
    [Header("Автозаполняемые списки")]
    [SerializeField] private List<Waypoint> _points = new();
    [SerializeField] private List<SectionRoadStrip> _connectedSections = new();

    public List<Waypoint> Points => new(_points);

    public List<SectionRoadStrip> ConnectedSections => new(_connectedSections);

    public void SetPoints(List<Waypoint> points) =>
        _points = new(points);

    public void SetConnectedSections(List<SectionRoadStrip> connectedSections) =>
        _connectedSections = new(connectedSections);

#if UNITY_EDITOR
    private const string Waypoint = nameof(Waypoint);
    public const int MinWaypointsCount = 2;

    [Header("Параметры только для редактора:")]
    [Header("Создаёт конкретное количество точек")]
    [SerializeField, Range(MinWaypointsCount, 100)] private int _count = 2;

    [Header("Вычисляет количество точек по расстоянию:")]
    [SerializeField] private bool _isDistanceDependency;
    [SerializeField, Range(0.05f, 100)] private float _distance = 1f;

    [Header("Автовыравнивание:")]
    [Tooltip("Если включено, выставляет точки от первой к последней на одинаковых интервалах")]
    [SerializeField] private bool _isAutoEqualizingPoint = true;
    [Tooltip("Добавляет изгибы линии.\n(Для разворота на 180 градусов лучше сделать двумя секциями с изгибом).\nПрименять в сочетании с включённым режимом \"Автовыравнивание точек\"")]
    [SerializeField] private Vector3 _arcValue = new();

    private readonly PointEqualizer _pointEqualizer = new();

    public void UpdateCount()
    {
        List<Transform> transforms = GetChildren();
        EnsureMinimumWaypoints(ref transforms);
        AdjustWaypointsToCount(ref transforms);
        RenameChildren(transforms);
        SortChildren(transforms);

        if (_isAutoEqualizingPoint)
            _pointEqualizer.EqualizeDistanceBetweenPoints(transforms, _arcValue);
    }

    private void EnsureMinimumWaypoints(ref List<Transform> transforms)
    {
        while (transforms.Count < MinWaypointsCount)
            AddNewWaypoint(ref transforms);
    }

    private void AdjustWaypointsToCount(ref List<Transform> transforms)
    {
        UpdateWaypointsCount(transforms);

        while (transforms.Count < _count)
            AddNewWaypoint(ref transforms);

        while (transforms.Count > _count)
            RemoveWaypoint(ref transforms);
    }

    private void UpdateWaypointsCount(List<Transform> transforms)
    {
        if (_isDistanceDependency == false)
            return;

        float distance = Vector3.Distance(transforms.First().position, transforms.Last().position);
        _count = Mathf.Max((int)(distance / _distance), MinWaypointsCount);
    }

    private void AddNewWaypoint(ref List<Transform> transforms)
    {
        GameObject newElement = new(Waypoint);
        newElement.AddComponent<Waypoint>();
        newElement.transform.SetParent(transform);
        int insertIndex = Mathf.Max(transforms.Count - 1, 0);
        transforms.Insert(insertIndex, newElement.transform);
    }

    private void RemoveWaypoint(ref List<Transform> transforms)
    {
        int secondLastIndex = transforms.Count - 2;
        DestroyImmediate(transforms[secondLastIndex].gameObject);
        transforms.RemoveAt(secondLastIndex);
    }

    private List<Transform> GetChildren()
    {
        List<Transform> children = new();
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
            if (child != transform && child.GetComponent<Waypoint>() != null)
                children.Add(child);

        return GetSortedByName(children);
    }

    private void RenameChildren(List<Transform> transforms)
    {
        for (int i = 0; i < transforms.Count; i++)
            transforms[i].name = $"{Waypoint}{i}";
    }

    private List<Transform> GetSortedByName(List<Transform> transforms)
    {
        if (transforms == null)
            return new List<Transform>();

        return transforms
            .Where(t => t != null)
            .OrderBy(t => Utils.ExtractName(t.name))
            .ToList();
    }

    private void SortChildren(List<Transform> transforms)
    {
        if (transforms == null)
            return;

        for (int i = 0; i < transforms.Count; i++)
            transforms[i].SetSiblingIndex(i);
    }
#endif
}