using UnityEngine;

public class DestinationMarker : MonoBehaviour 
{
    [SerializeField] private Vector3 _offset;

    public void SetPosition(Vector3 position) =>
        transform.position = position + _offset;
}