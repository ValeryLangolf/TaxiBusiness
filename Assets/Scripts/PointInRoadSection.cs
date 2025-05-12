using UnityEngine;

public class PointInRoadSection
{
    private SectionRoadStrip _section;
    private Transform _point;

    public PointInRoadSection(SectionRoadStrip section, Transform point)
    {
        _section = section;
        _point = point;
    }

    public SectionRoadStrip Section => _section;

    public Transform Point => _point;

    public Vector3 Position => _point.position;

    public void SetSection(SectionRoadStrip section) =>
        _section = section;

    public void SetPoint(Transform point) =>
        _point = point;
}