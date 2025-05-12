#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class RoadBuilder : MonoBehaviour
{
    [SerializeField] private RoadNetwork _roadNetwork;

    [SerializeField] private Color _sectionColor;
    [SerializeField] private Color _connectionColor;
    [SerializeField] private float _waypointSphereRadius;
    [SerializeField] private float _connectionSphereRadius;
    [SerializeField] private float _connectDistance;

    [Space(10)]
    [SerializeField] private bool _isShowGizmos;
    [SerializeField] private bool _isAutoConnect;
    [SerializeField] private bool _isAutoFind;

    private readonly List<(Vector3 position, float radius, GameObject linkedObject)> _clickableSpheres = new();
    private readonly GizmosRoadDrower _drower = new();
    private readonly RoadConnector _connector = new();

    public List<SectionRoadStrip> Sections => new(_roadNetwork.Sections);

    private void OnValidate()
    {
        if (_isShowGizmos)
        {
            _drower.SetParams(_sectionColor, _connectionColor, _waypointSphereRadius, _connectionSphereRadius);
            SceneView.duringSceneGui += OnSceneGUI;
        }

        if (_isAutoConnect)
            _connector.SetParams(_connectDistance);
    }

    private void Update()
    {
        if (_isAutoFind)
            Find();

        if (_isAutoConnect)
            _connector.Connect(_roadNetwork.Sections);
    }

    private void OnDrawGizmos()
    {
        List<SectionRoadStrip> sections = _roadNetwork.Sections;

        if (_isShowGizmos == false || sections.Count == 0)
            return;

        _drower.Drow(sections);
        _clickableSpheres.Clear();

        foreach (SectionRoadStrip section in sections)
            SetClickable(section);
    }

    public void SetGizmosFlag(bool isOn) =>
        _isShowGizmos = isOn;
    
    public void SetAutoFindFlag(bool isOn) =>
        _isAutoFind = isOn;
    
    public void SetAutoConnectFlag(bool isOn) =>
        _isAutoConnect = isOn;

    private void SetClickable(SectionRoadStrip section)
    {
        if (section == null)
        {
            Debug.Log("Исключение нулевой ссылки. Секция не существует. Обнови дорожную сеть!");
            return;
        }

        IReadOnlyList<Transform> points = section.Points;

        if (points == null || points.Count == 0)
            return;

        for (int i = 0; i < points.Count; i++)
        {
            if (section.Points[i] == null)
                continue;

            _clickableSpheres.Add((points[i].position, _waypointSphereRadius, points[i].gameObject));
        }
    }

    public void ConnectAllLanes()
    {
        Find();
        _connector.Connect(_roadNetwork.Sections);
        Debug.Log($"ОБЪЕДИНЕНИЕ СЕКЦИЙ В ЕДИНУЮ ДОРОЖНУЮ СЕТЬ. Количество секций: {_roadNetwork.Sections.Count}");
    }

    private void Find()
    {
        FindSections();
        FindPointsInSections();
    }

    private void FindSections()
    {
        List<SectionRoadStrip> sections = GetComponentsInChildren<SectionRoadStrip>(true).ToList();

        if (Utils.AreListsEqual(_roadNetwork.Sections, sections))
            return;

        _roadNetwork.SetSections(sections);
        EditorUtility.SetDirty(_roadNetwork);
    }

    private void FindPointsInSections()
    {
        foreach (SectionRoadStrip section in _roadNetwork.Sections)
        {
            List<Transform> points = section.GetComponentsInChildren<Transform>()
                .Where(child => child != section.transform)
                .OrderBy(child => Utils.ExtractName(child.name))
                .ToList();

            if (section.Points.SequenceEqual(points))
                continue;

            section.SetPoints(points);
            EditorUtility.SetDirty(section);
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