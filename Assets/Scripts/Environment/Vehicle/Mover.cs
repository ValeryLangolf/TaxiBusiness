using UnityEngine;

public class Mover
{
    private readonly Transform _vehicle;

    public Mover(Transform vehicle)
    {
        _vehicle = vehicle;
    }

    public void Move(Vector3 target, float speed)
    {
        target.y = _vehicle.position.y;
        _vehicle.position = Vector3.MoveTowards(_vehicle.position, target, speed * Time.deltaTime);
    }
}