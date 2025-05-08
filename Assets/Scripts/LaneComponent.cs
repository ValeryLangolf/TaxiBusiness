using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneComponent : MonoBehaviour
{
    [SerializeField] private List<Transform> _points = new();
    [SerializeField] private LaneType _type = LaneType.Main;
    [SerializeField][Range(0, 120)] private float _speedLimit = 60f;
    [SerializeField] private List<LaneComponent> _connectedLanes = new();
    [SerializeField] private bool _isOneWay = false;

    [Header("Gizmos Settings")]
    [SerializeField] private Color _connectionColor = Color.yellow;
    [SerializeField] private bool _showGizmos = true;
    [SerializeField] private float _sphereRadius = 0.5f;
    [SerializeField] private float _connectionSphereSize = 0.4f;
    [SerializeField] private float _autoConnectDistance = 10f;

    public IReadOnlyList<Transform> Points => _points;
    public LaneType Type => _type;
    public IReadOnlyList<LaneComponent> ConnectedLanes => _connectedLanes;
    public float SpeedLimit => _speedLimit;
    public bool IsOneWay => _isOneWay;

    private void OnDrawGizmos()
    {
        DrawLanePath();
        DrawConnections();
    }

    private Color GetGizmoColorByType() => _type switch
    {
        LaneType.Main => Color.blue,
        LaneType.LeftTurn => Color.green,
        LaneType.RightTurn => Color.red,
        LaneType.Bus => Color.yellow,
        LaneType.Emergency => Color.magenta,
        LaneType.Opposite => new Color(1, 0.5f, 0), // оранжевый
        LaneType.Transition => Color.cyan,
        _ => Color.white
    };

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

    public int GetPointIndex(Transform point)
    {
        return _points.FindIndex(p => p == point);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto Connect Lanes")]
    public void AutoConnectLanes()
    {
        _connectedLanes.Clear();

        if (_points == null || _points.Count == 0)
        {
            Debug.LogWarning("No points in lane!", this);
            return;
        }

        Vector3 endPoint = _points[^1].position;

        foreach (LaneComponent otherLane in FindObjectsByType<LaneComponent>(FindObjectsSortMode.None))
        {
            if (otherLane == this) continue;
            if (otherLane.Points == null || otherLane.Points.Count == 0) continue;

            Vector3 otherStart = otherLane.Points[0].position;
            float distance = Vector3.Distance(endPoint, otherStart);

            if (distance <= _autoConnectDistance)
            {
                _connectedLanes.Add(otherLane);
                Debug.Log($"Connected {name} to {otherLane.name} ({distance:F1}m)");
            }
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    private void DrawLanePath()
    {
        if (!_showGizmos || _points == null || _points.Count < 2)
            return;

        Gizmos.color = GetGizmoColorByType();

        // Рисуем линии между точками
        for (int i = 0; i < _points.Count - 1; i++)
        {
            if (_points[i] == null || _points[i + 1] == null)
                continue;

            Gizmos.DrawLine(_points[i].position, _points[i + 1].position);
            Gizmos.DrawSphere(_points[i].position, _sphereRadius);
        }

        // Последняя сфера
        if (_points[^1] != null)
            Gizmos.DrawSphere(_points[^1].position, _sphereRadius);
    }

    private void DrawConnections()
    {
        if (_connectedLanes == null || _connectedLanes.Count == 0)
            return;

        Gizmos.color = _connectionColor;

        foreach (LaneComponent connectedLane in _connectedLanes)
        {
            if (connectedLane == null) continue;
            if (this.Points.Count == 0 || connectedLane.Points.Count == 0) continue;

            Vector3 startPoint = this.Points.Last().position;
            Vector3 endPoint = connectedLane.Points.First().position;

            // Линия соединения
            Gizmos.DrawLine(startPoint, endPoint);

            // Сферы на концах
            Gizmos.DrawWireSphere(startPoint, _connectionSphereSize);
            Gizmos.DrawWireSphere(endPoint, _connectionSphereSize);

            // Подпись с расстоянием
#if UNITY_EDITOR
            UnityEditor.Handles.Label(
                Vector3.Lerp(startPoint, endPoint, 0.5f),
                $"{Vector3.Distance(startPoint, endPoint):F1}m",
                new GUIStyle { normal = new GUIStyleState { textColor = _connectionColor } }
            );
#endif
        }
    }
}