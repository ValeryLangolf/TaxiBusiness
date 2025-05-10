using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private RoadNetwork _roadNetwork;
    [SerializeField] private Pathfinder _pathfinder;
    [SerializeField] private VehicleController _vehiclePrefab;

    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _customEndPoint;

    private void Awake()
    {
        if (_roadNetwork == null)
            throw new NullReferenceException("_roadManager отсутствует ссылка на компонент");

        if (_pathfinder == null)
            throw new NullReferenceException("_pathfinder отсутствует ссылка на компонент");

        if (_vehiclePrefab == null)
            throw new NullReferenceException("_vehiclePrefab отсутствует ссылка на компонент");
    }

    private void Start() =>
        SpawnVehicle();

    private void SpawnVehicle()
    {
        Instantiate(_vehiclePrefab, _startPosition.position, _startPosition.rotation);
    }
}