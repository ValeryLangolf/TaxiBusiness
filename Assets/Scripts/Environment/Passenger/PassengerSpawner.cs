using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private Passenger _prefab;
    [SerializeField] private Vector2 _timeLimitInSeconds;
    [SerializeField] private BoosterPanel _boosterPanel;

    private Pool<Passenger> _pool;
    private List<Waypoint> _points;
    private readonly List<Passenger> _passengers = new();

    private void Awake() =>
        _pool = new(_prefab, transform);

    private void Start()
    {
        _points = RoadNetwork.Instance.Points.Where(p => p.IsNotForPassenger == false).ToList();
        StartCoroutine(Spawning());
    }

    public List<Passenger> GetAwailable() =>
        _passengers.Where(p => p.IsSelect == false).ToList();

    private IEnumerator Spawning()
    {
        while (true)
        {
            float waitTime = Random.Range(_timeLimitInSeconds.x, _timeLimitInSeconds.y) * _boosterPanel.PassengerMultiplier;
            yield return new WaitForSeconds(waitTime);

            Spawn();
        }
    }

    private void Spawn()
    {
        if (_pool.TryGet(out Passenger passenger) == false)
            return;

        List<Waypoint> remainingPoints = new(_points);
        Waypoint position = GiveRandomWaypoint(remainingPoints);
        Waypoint destination = GiveRandomWaypoint(remainingPoints);

        passenger.SetDeparture(position);
        passenger.SetDestination(destination);

        SubscribePassenger(passenger);
    }

    private Waypoint GiveRandomWaypoint(List<Waypoint> points)
    {
        Waypoint waypoint = points[Random.Range(0, points.Count)];
        points.Remove(waypoint);

        return waypoint;
    }

    private void SubscribePassenger(Passenger passenger)
    {
        passenger.Deactivated += OnPassengerDisabled;
        passenger.Taked += OnPassengerDisabled;
        _passengers.Add(passenger);
    }

    private void UnsubscribePassenger(Passenger passenger)
    {
        passenger.Deactivated -= OnPassengerDisabled;
        passenger.Taked -= OnPassengerDisabled;
        _passengers.Remove(passenger);
    }

    private void OnPassengerDisabled(Passenger passenger) =>
        UnsubscribePassenger(passenger);
}