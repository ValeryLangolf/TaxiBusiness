using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private Passenger _prefab;
    [SerializeField] private Vector2 _timeLimitInSeconds;

    private Pool<Passenger> _pool;
    private readonly List<Passenger> _passengers = new();

    public List<Passenger> Passengers => new(_passengers);

    private void Awake() =>
        _pool = new(_prefab, transform);

    private void Start() =>
        StartCoroutine(Spawning());

    public List<Passenger> GetAwailable() =>
        _passengers.Where(p => p.IsSelect == false).ToList();    

    private IEnumerator Spawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_timeLimitInSeconds.x, _timeLimitInSeconds.y));

            Spawn();
        }
    }

    private void Spawn()
    {
        if (_pool.TryGet(out Passenger passenger) == false)
            return;

        Waypoint position = GetRandomWaypoint();
        Waypoint destination = GetRandomWaypoint();

        while(position == destination)
            destination = GetRandomWaypoint();
        
        passenger.SetDeparture(position);
        passenger.SetDestination(destination);
        
        SubscribePassenger(passenger);
    }

    private Waypoint GetRandomWaypoint()
    {
        List<Waypoint> points = RoadNetwork.Instance.Points;
        int id = Random.Range(0, points.Count);

        return points[id];
    }

    private void SubscribePassenger(Passenger passenger)
    {
        passenger.Deactivated += OnPassengerDisabled;
        passenger.Taked += OnPassengerDisabled;

        _passengers.Add(passenger);
    }

    private void OnPassengerDisabled(Passenger passenger)
    {
        passenger.Deactivated -= OnPassengerDisabled;
        passenger.Taked -= OnPassengerDisabled;

        _passengers.Remove(passenger);
    }
}