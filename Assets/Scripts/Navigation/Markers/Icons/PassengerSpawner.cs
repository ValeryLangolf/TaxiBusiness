using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private Passenger _prefab;
    [SerializeField] private Vector2 _timeLimitInSeconds;

    private Pool<Passenger> _pool;

    private void Awake() =>
        _pool = new(_prefab, transform);

    private void Start() =>
        StartCoroutine(Spawning());

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
        if (_pool.TryGet(out Passenger passenger))
            passenger.Follow(GetRandomPosition());
    }

    private Transform GetRandomPosition()
    {
        List<Waypoint> points = RoadNetwork.Instance.Points;
        int id = Random.Range(0, points.Count);

        return points[id].transform;
    }
}