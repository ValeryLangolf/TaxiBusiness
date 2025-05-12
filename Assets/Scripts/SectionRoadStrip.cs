using System.Collections.Generic;
using UnityEngine;

public class SectionRoadStrip : MonoBehaviour
{
    [Header("Автозаполняемые списки")]
    [SerializeField] private List<Transform> _points = new();
    [SerializeField] private List<SectionRoadStrip> _connectedSections = new();

    public List<Transform> Points => new(_points);

    public List<SectionRoadStrip> ConnectedSections => new(_connectedSections);

    public float LenghtInMeter => CalculateLenght();


    public void SetPoints(List<Transform> points) =>
        _points = new(points);

    public void SetConnectedSections(List<SectionRoadStrip> connectedSections) =>
        _connectedSections = new(connectedSections);

    public int GetPointIndex(Transform point) =>
        _points.FindIndex(p => p == point);

    public Transform GetClosestPoint(Vector3 position)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform point in _points)
        {
            float dist = Vector3.Distance(position, point.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                closest = point;
            }
        }

        return closest;
    }

    private float CalculateLenght()
    {
        float distance = 0;

        for (int i = 1; i < _points.Count; i++)
            distance = Vector3.Distance(_points[i - 1].position, _points[i].position);

        return distance;
    }
}