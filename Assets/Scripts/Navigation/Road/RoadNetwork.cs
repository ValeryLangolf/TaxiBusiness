#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    public static RoadNetwork Instance { get; private set; }

    [Header("Автозаполняемый список")]
    [SerializeField] private List<SectionRoadStrip> _sections;

    private readonly List<Waypoint> _points = new();

    public List<SectionRoadStrip> Sections => new(_sections);

    public List<Waypoint> Points => new(_points);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ReconstructByPoints();
    }

    public void SetSections(List<SectionRoadStrip> sections) =>
        _sections = new(sections);

    private void ReconstructByPoints()
    {
        AddPointsToList();

        foreach (SectionRoadStrip section in _sections)
        {
            List<Waypoint> waypoints = section.Points;

            for (int i = 0; i < waypoints.Count; i++)
            {
                if (i + 1 < waypoints.Count)
                    waypoints[i].AddConnectedPoint(waypoints[i + 1]);

                if (i == waypoints.Count - 1)
                    ConnectLastWaypointToConnectedSections(waypoints[i], section.ConnectedSections);
            }
        }
    }

    private void AddPointsToList()
    {
        foreach (SectionRoadStrip section in _sections)
            foreach (Waypoint waypoint in section.Points)
                _points.Add(waypoint);
    }

    private void ConnectLastWaypointToConnectedSections(Waypoint lastWaypoint, List<SectionRoadStrip> connectedSections)
    {
        foreach (SectionRoadStrip connectedSection in connectedSections)
            lastWaypoint.AddConnectedPoint(connectedSection.Points.First());
    }

#if UNITY_EDITOR
    [Header("Параметры только для редактора")]
    [SerializeField] private Color _notPassengerColor = Color.red;
    [SerializeField] private Color _sectionColor = Color.green;
    [SerializeField] private Color _connectionColor = Color.yellow;
    [SerializeField] private Color _notConnectColor = Color.magenta;
    [SerializeField] private float _waypointSphereRadius = 0.09f;
    [SerializeField] private float _connectionSphereRadius = 0.05f;
    [SerializeField] private float _connectDistance = 0.5f;
    [SerializeField] private float _notConnectSphereRadius = 1.2f;

    private bool _isShowGizmos;
    private bool _isNotConnect;

    private readonly List<(Vector3 position, float radius, GameObject linkedObject)> _clickableSpheres = new();
    private readonly GizmosRoadDrawer _drawer = new();
    private readonly RoadConnector _connector = new();

    private void OnDrawGizmos()
    {
        if (_isShowGizmos)
        {
            List<SectionRoadStrip> sections = new(_sections);

            if (_isShowGizmos == false || sections.Count == 0)
                return;

            _drawer.SetParams(_sectionColor, _notPassengerColor, _connectionColor, _waypointSphereRadius, _connectionSphereRadius);
            _drawer.Draw(sections);
            _clickableSpheres.Clear();

            foreach (SectionRoadStrip section in sections)
                SetClickable(section);
        }

        if (_isNotConnect)
            _drawer.DrawSpheresNotConnection(_notConnectSphereRadius, _notConnectColor, _sections);
    }

    public void SetGizmosFlag(bool isOn) =>
        _isShowGizmos = isOn;

    public void SetFlagNotConnectPoints(bool isOn) =>
        _isNotConnect = isOn;

    public void UpdateSections()
    {
        List<SectionRoadStrip> newSections = GetComponentsInChildren<SectionRoadStrip>().ToList();
        List<SectionRoadStrip> currentSections = new(_sections);

        if (Utils.HasChanges(newSections, currentSections))
        {
            SetSections(newSections);
            EditorUtility.SetDirty(this);
            Debug.Log("Найдены изменения в количсестве секций. Данные обновлены автоматически");
        }

        UpdatePoints();
    }

    public void ConnectPoints() =>
        _connector.Connect(new(_sections), _connectDistance);

    public void ConnectAllLanes()
    {
        _connector.Connect(new(_sections), _connectDistance);

        int countPoint = 0;

        foreach (SectionRoadStrip section in _sections)
            countPoint += section.Points.Count;

        Debug.Log($"ОБЪЕДИНЕНИЕ СЕКЦИЙ В ЕДИНУЮ ДОРОЖНУЮ СЕТЬ!");
        Debug.Log($"Количество секций: {_sections.Count}");
        Debug.Log($"Количество точек: {countPoint}");
    }

    private void SetClickable(SectionRoadStrip section)
    {
        if (section == null)
            return;

        List<Waypoint> points = section.Points;

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
        foreach (SectionRoadStrip section in _sections)
        {
            List<Waypoint> points = section.GetComponentsInChildren<Waypoint>().ToList();

            if (Utils.HasChanges(points, section.Points) == false)
                continue;

            section.SetPoints(points);
            EditorUtility.SetDirty(section);
            Debug.Log($"Найдены изменения количества точек в секции \"{section.name}\". Данные обновлены автоматически");
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
#endif
}