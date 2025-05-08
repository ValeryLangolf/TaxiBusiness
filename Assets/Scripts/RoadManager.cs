using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private List<LaneComponent> _allLanes = new();

    private void Awake()
    {
        _allLanes = new List<LaneComponent>(FindObjectsByType<LaneComponent>(FindObjectsSortMode.None));
    }

    public void RegisterLane(LaneComponent lane)
    {
        if (!_allLanes.Contains(lane))
            _allLanes.Add(lane);
    }

    public void UnregisterLane(LaneComponent lane)
    {
        if (_allLanes.Contains(lane))
            _allLanes.Remove(lane);
    }

    public List<LaneComponent> GetAllLanes() => new(_allLanes);

    public List<LaneComponent> GetLanesByType(LaneType type) =>
        _allLanes.FindAll(l => l.Type == type);
}