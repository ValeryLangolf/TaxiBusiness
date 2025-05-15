using UnityEngine;

public class Rotator
{
    private readonly Transform _vehicle;

    public Rotator(Transform vehicle)
    {
        _vehicle = vehicle;
    }

    public void Rotate(Vector3 target)
    {
        target.y = _vehicle.position.y;

        if (target == _vehicle.position)
        {
            Debug.Log("Цель равна текущей позиции");
            return;
        }
            

        Vector3 direction = (target - _vehicle.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _vehicle.rotation = targetRotation;
    }
}