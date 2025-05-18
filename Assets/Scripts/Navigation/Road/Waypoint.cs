using System.Collections.Generic;
using UnityEngine;

public enum PassengerCategory
{
    Economy,
    Standard,
    Comfort,
    Business,
    Luxury,
    VIP,
    PetTransport,
    WithChildSeat
}

public class Waypoint : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _economy;
    [SerializeField, Range(0, 1)] private float _standard;
    [SerializeField, Range(0, 1)] private float _comfort;
    [SerializeField, Range(0, 1)] private float _business;
    [SerializeField, Range(0, 1)] private float _luxury;
    [SerializeField, Range(0, 1)] private float _vip;
    [SerializeField, Range(0, 1)] private float _petTransport;
    [SerializeField, Range(0, 1)] private float _withChildSeat;

    private readonly List<Waypoint> _connectedWaypoints = new();

    public List<Waypoint> ConnectedPoints => _connectedWaypoints;

    public float LenghtInMeter => CalculateLenght();

    public Vector3 Position => transform.position;

    public void AddConnectedPoint(Waypoint waypoint) =>
        _connectedWaypoints.Add(waypoint);

    private float CalculateLenght() =>
        _connectedWaypoints.Count > 0 ? Vector3.Distance(transform.position, _connectedWaypoints[0].Position) : int.MaxValue;
}