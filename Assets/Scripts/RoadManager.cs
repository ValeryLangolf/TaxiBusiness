using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private List<SectionRoadStrip> _allLanes = new();

    private void Awake() =>
        _allLanes = new List<SectionRoadStrip>(FindObjectsByType<SectionRoadStrip>(FindObjectsSortMode.None));

    public List<SectionRoadStrip> GetAllLanes() => new(_allLanes);
}