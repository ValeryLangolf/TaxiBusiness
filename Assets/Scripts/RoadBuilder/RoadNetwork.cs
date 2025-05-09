using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class RoadNetwork : MonoBehaviour
{
    [Header("Auto-filled")]
    [SerializeField] private List<SectionRoadStrip> _sections = new();

    [Header("Gizmos Settings")]
    [SerializeField] private Color _sectionColor;
    [SerializeField] private Color _connectionColor;
    [SerializeField] private float _waypointSphereRadius;
    [SerializeField] private float _connectionSphereRadius;
    [SerializeField] private float _connectDistance;
    [SerializeField] private bool _isShowGizmos;
    [SerializeField] private bool _isSubscribe;

    private readonly List<(Vector3 position, float radius, GameObject linkedObject)> _clickableSpheres = new();
    private Drower _drower = new();

    public List<SectionRoadStrip> Sections => new(_sections);

    private void OnValidate()
    {
        if (_isSubscribe)
            Subscribe();
        else
            Unsubscribe();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_isShowGizmos == false || _sections.Count == 0)
            return;

        _clickableSpheres.Clear();

        _drower.DrowSections(_sections, _sectionColor);

        foreach (SectionRoadStrip section in _sections)
            DrowSection(section);

        foreach (SectionRoadStrip section in _sections)
            DrowConnections(section);
    }

    private void DrowSection(SectionRoadStrip section)
    {
        if (section.Points == null || section.Points.Count < 2)
            return;

        Gizmos.color = _sectionColor;
        IReadOnlyList<Transform> points = section.Points;

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (section.Points[i] == null || points[i + 1] == null)
                continue;

            //Gizmos.DrawLine(points[i].position, points[i + 1].position);
            DrawClickableSphere(_waypointSphereRadius, points[i].gameObject);
        }

        if (points[^1] != null)
            DrawClickableSphere(_waypointSphereRadius, points[^1].gameObject);
    }

    private void DrowConnections(SectionRoadStrip section)
    {
        IReadOnlyList<SectionRoadStrip> connectedLanes = section.ConnectedLanes;

        if (connectedLanes == null || connectedLanes.Count == 0)
            return;

        Gizmos.color = _connectionColor;

        foreach (SectionRoadStrip connectedLane in connectedLanes)
        {
            if (connectedLane == null)
                continue;

            if (section.Points.Count == 0 || connectedLane.Points.Count == 0)
                continue;

            Transform startPoint = section.Points.Last();
            Transform endPoint = connectedLane.Points.First();

            Gizmos.DrawLine(startPoint.position, endPoint.position);
            DrawClickableSphere(_connectionSphereRadius, startPoint.gameObject);
            DrawClickableSphere(_connectionSphereRadius, endPoint.gameObject);
        }
    }

    public void ConnectAllLanes()
    {
        FindSections();
        ConnectSections();

        EditorUtility.SetDirty(this);
        Debug.Log($"ОБЪЕДИНЕНИЕ СЕКЦИЙ В ЕДИНУЮ ДОРОЖНУЮ СЕТЬ. Количество секций: {_sections.Count}");
    }

    private void Subscribe()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        Debug.Log($"Подписался");
    }

    private void Unsubscribe()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        Debug.Log($"Отписался");
    }

    private void FindSections()
    {
        _sections = GetComponentsInChildren<SectionRoadStrip>(true).ToList();
        FindPointsInSections();
    }

    private void FindPointsInSections()
    {
        foreach (SectionRoadStrip section in _sections)
        {
            List<Transform> points = section.GetComponentsInChildren<Transform>()
                .Where(child => child != section.transform)
                .OrderBy(child => child.name)
                .ToList();

            section.SetPoints(points);
        }
    }

    private void ConnectSections()
    {
        foreach (SectionRoadStrip section in _sections)
        {
            List<SectionRoadStrip> connectedSections = new();
            IReadOnlyList<Transform> points = section.Points;

            if (points == null || points.Count == 0)
            {
                Debug.LogWarning($"В секции \"{section.gameObject.name}\" не найдено точек пути!", section);
                return;
            }

            Vector3 endPoint = points[^1].position;

            foreach (SectionRoadStrip otherSection in _sections)
            {
                if (otherSection == section)
                    continue;

                IReadOnlyList<Transform> otherSectionPoints = otherSection.Points;

                if (otherSectionPoints == null || otherSectionPoints.Count == 0)
                    continue;

                Vector3 otherStart = otherSectionPoints[0].position;
                float distance = Vector3.Distance(endPoint, otherStart);

                if (distance <= _connectDistance)
                    connectedSections.Add(otherSection);
            }

            section.SetConnectedSections(connectedSections);
            EditorUtility.SetDirty(section);
        }
    }

    private void DrawClickableSphere(float radius, GameObject linkedObject)
    {
        Handles.color = Gizmos.color;
        Vector3 position = linkedObject.transform.position;
        float diameter = radius * 2;

        _clickableSpheres.Add((position, diameter, linkedObject));

        Handles.SphereHandleCap(
            0,
            position,
            Quaternion.identity,
            diameter,
            EventType.Repaint
        );
    }

    public void OnSceneGUI(SceneView _)
    {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
        {
            Debug.Log($"Сработало сколько раз?");

            Vector2 mousePosition = currentEvent.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

            foreach (var sphere in _clickableSpheres)
            {
                Vector3 sphereCenter = sphere.position;
                float radius = sphere.radius;

                Vector3 closestPoint = ray.origin + ray.direction * Vector3.Dot(ray.direction, sphereCenter - ray.origin);
                float distanceToSphere = Vector3.Distance(closestPoint, sphereCenter);

                if (distanceToSphere <= radius)
                {
                    Selection.activeGameObject = sphere.linkedObject;
                    EditorGUIUtility.PingObject(sphere.linkedObject);
                    currentEvent.Use();

                    break;
                }
            }
        }
    }
#endif
}