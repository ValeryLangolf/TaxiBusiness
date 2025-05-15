using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private SectionRoadStrip _section;

    public SectionRoadStrip Section => _section;

    public Vector3 Position => transform.position;

    private void Awake() =>
        _section = GetComponentInParent<SectionRoadStrip>(true);
}