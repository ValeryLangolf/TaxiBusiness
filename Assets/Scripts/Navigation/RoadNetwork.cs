using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    [Header("Автозаполняемый список")]
    [SerializeField] private List<SectionRoadStrip> _sections = new();

    public static RoadNetwork Instance { get; private set; }

    public List<SectionRoadStrip> Sections => new(_sections);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetSections(List<SectionRoadStrip> sections) =>
        _sections = new(sections);
}