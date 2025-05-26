#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class RoadBuilder : MonoBehaviour
{
    [SerializeField] private RoadNetwork _roadNet;
    [SerializeField] private Color _notPassengerColor;
    [SerializeField] private Color _sectionColor;
    [SerializeField] private Color _connectionColor;
    [SerializeField] private float _waypointSphereRadius;
    [SerializeField] private float _connectionSphereRadius;
    [SerializeField] private float _connectDistance;

    private bool _isShowGizmos;

    private readonly List<(Vector3 position, float radius, GameObject linkedObject)> _clickableSpheres = new();
    private readonly GizmosRoadDrower _drower = new();
    private readonly RoadConnector _connector = new();

    public List<SectionRoadStrip> Sections => new(_roadNet.Sections);

    private void OnDrawGizmos()
    {
        if (_isShowGizmos == false)
            return;

        List<SectionRoadStrip> sections = _roadNet.Sections;

        if (_isShowGizmos == false || sections.Count == 0)
            return;

        _drower.SetParams(_sectionColor, _notPassengerColor, _connectionColor, _waypointSphereRadius, _connectionSphereRadius);
        _drower.Drow(sections);
        _clickableSpheres.Clear();

        foreach (SectionRoadStrip section in sections)
            SetClickable(section);
    }

    public void SetGizmosFlag(bool isOn) =>
        _isShowGizmos = isOn;

    public void UpdateSections()
    {
        List<SectionRoadStrip> currentSections = GetComponentsInChildren<SectionRoadStrip>().ToList();

        if (Utils.HasChanges(currentSections, _roadNet.Sections))
        {
            _roadNet.SetSections(currentSections);
            EditorUtility.SetDirty(_roadNet);

            Debug.Log("Обновлены секции дорожной сети!");
        }

        UpdatePoints();
    }

    public void ConnectPoints() =>
        _connector.Connect(_roadNet.Sections, _connectDistance);

    public void ConnectAllLanes()
    {
        _connector.Connect(_roadNet.Sections, _connectDistance);

        int countPoint = 0;

        foreach (SectionRoadStrip section in _roadNet.Sections)
            countPoint += section.Points.Count;

        Debug.Log($"ОБЪЕДИНЕНИЕ СЕКЦИЙ В ЕДИНУЮ ДОРОЖНУЮ СЕТЬ!");
        Debug.Log($"Количество секций: {_roadNet.Sections.Count}");
        Debug.Log($"Количество точек: {countPoint}");
    }

    private void SetClickable(SectionRoadStrip section)
    {
        if (section == null)
        {
            Debug.Log("Исключение нулевой ссылки. Секция не существует. Обнови дорожную сеть!");
            return;
        }

        IReadOnlyList<Waypoint> points = section.Points;

        if (points == null || points.Count == 0)
            return;

        for (int i = 0; i < points.Count; i++)
        {
            if (section.Points[i] == null)
                continue;

            _clickableSpheres.Add((points[i].Position, _waypointSphereRadius, points[i].gameObject));
        }
    }

    private void UpdatePoints()
    {
        foreach (SectionRoadStrip section in _roadNet.Sections)
        {
            List<Waypoint> points = section.GetComponentsInChildren<Waypoint>().ToList();

            if (Utils.HasChanges(points, section.Points) == false)
                continue;

            section.SetPoints(points);
            EditorUtility.SetDirty(section);
            Debug.Log($"Обновлены точки секции {section.name}");
        }
    }

    public void OnSceneGUI(SceneView _)
    {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
        {
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
}
#endif