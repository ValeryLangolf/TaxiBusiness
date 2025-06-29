using UnityEngine;

public class DriverMan : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _shareOfRevenue;

    public Sprite Sprite => _sprite;

    public float ShareOfRevenue => _shareOfRevenue;
}