using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathDrawerView : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Clear();
    }

    public void SetColor(Color color)
    {
        if (_lineRenderer.startColor == color)
            return;

        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    public void Clear() =>
        Draw(new());

    public void Draw(List<Vector3> path)
    {
        _lineRenderer.positionCount = path?.Count ?? 0;

        if (path != null && path.Count > 0)
            _lineRenderer.SetPositions(path.ToArray());
    }
}