using UnityEngine;

public class DestinationPointClickSpawner : MonoBehaviour
{
    [SerializeField] private VehicleDispatcher _dispatcher;
    [SerializeField] private DestinationPointClickMarker _prefab;
    [SerializeField] private Vector3 _offset;

    private Pool<DestinationPointClickMarker> _pool;

    private void Awake() =>
        _pool = new(_prefab, transform);

    private void OnEnable() =>
        _dispatcher.PlaneClicked += OnPlaneClicked;

    private void OnDisable() =>
        _dispatcher.PlaneClicked -= OnPlaneClicked;

    private void OnPlaneClicked(Vector3 position)
    {
        if(_pool.TryGet(out DestinationPointClickMarker marker))
            marker.SetPosition(position + _offset);
    }
}