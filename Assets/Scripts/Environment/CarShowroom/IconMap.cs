using UnityEngine;

[RequireComponent(typeof(UiFollower))]
public class IconMap : MonoBehaviour
{
    [SerializeField] private MyCastomTarget _target;

    private UiFollower _follower;

    private void Awake() =>
        _follower = GetComponent<UiFollower>();

    private void Start() =>
        _follower.Follow(_target.transform);
}