using UnityEngine;

public class DestinationPointClickSpawner : MonoBehaviour
{
    [SerializeField] private DestinationPointClickMarker _prefab;
    [SerializeField] private Vector3 _offset;

    private Pool<DestinationPointClickMarker> _pool;

    private void Awake() =>
        _pool = new(_prefab, transform);

    private void OnEnable() =>
        VehicleDispatcher.PlaneClicked += OnPlaneClicked;

    private void OnDisable() =>
        VehicleDispatcher.PlaneClicked -= OnPlaneClicked;

    private void OnPlaneClicked(Vector3 position)
    {
        if(_pool.TryGet(out DestinationPointClickMarker marker))
            marker.SetPosition(position + _offset);
    }
}