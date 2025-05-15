using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private readonly List<Waypoint> _connectedWaypoints = new();

    public List<Waypoint> ConnectedPoints => _connectedWaypoints;

    public float LenghtInMeter => CalculateLenght();

    public Vector3 Position => transform.position;

    public void AddConnectedPoint(Waypoint waypoint) =>
        _connectedWaypoints.Add(waypoint);

    private float CalculateLenght()
    {
        if (_connectedWaypoints.Count > 0)
            return Vector3.Distance(transform.position, _connectedWaypoints[0].Position);

        return int.MaxValue;
    }
}