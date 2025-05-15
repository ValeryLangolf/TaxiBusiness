using UnityEngine;

public class Mover
{
    private readonly Transform _vehicle;
    private readonly float _speed;

    public Mover(Transform vehicle, float speed)
    {
        _vehicle = vehicle;
        _speed = speed;
    }

    public void Move(Vector3 target)
    {
        target.y = _vehicle.position.y;

        _vehicle.position = Vector3.MoveTowards(_vehicle.position, target, _speed * Time.deltaTime);
    }
}