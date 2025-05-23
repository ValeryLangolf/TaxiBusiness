using UnityEngine;

public class DestinationPointSpawner : MonoBehaviour
{
    [SerializeField] private DestinationPointClickMarker _prefab;
    [SerializeField] private Vector3 _offset;

    private Pool<DestinationPointClickMarker> _pool;

    private void Awake() =>
        _pool = new(_prefab, transform);

    public void Spawn(Vector3 position)
    {
        if(_pool.TryGet(out DestinationPointClickMarker marker))
            marker.SetPosition(position + _offset);
    }
}