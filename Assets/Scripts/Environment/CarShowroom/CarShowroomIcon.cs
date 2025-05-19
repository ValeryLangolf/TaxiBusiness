using UnityEngine;

[RequireComponent(typeof(UiFollower))]
public class CarShowroomIcon : MonoBehaviour
{
    [SerializeField] private CarShowroom _target;

    private UiFollower _follower;

    private void Awake() =>
        _follower = GetComponent<UiFollower>();

    private void Start() =>
        _follower.Follow(_target.transform);
}