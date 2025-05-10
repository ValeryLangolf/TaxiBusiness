using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    [Header("Автозаполняемый список")]
    [SerializeField] private List<SectionRoadStrip> _sections = new();

    public List<SectionRoadStrip> Sections => new(_sections);

    public void SetSections(List<SectionRoadStrip> sections) =>
        _sections = new(sections);
}