using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UiFollower))]
public class Passenger : MonoBehaviour, IDeactivatable<Passenger>
{
    private const string IsHide = nameof(IsHide);

    [SerializeField] private Vector2 _timeVisibleLimits;
    [SerializeField] private Animator _animator;
    [SerializeField] private PassengerAnimationEvent _event;
    
    private UiFollower _follower;

    public event Action<Passenger> Deactivated;

    private void Awake() =>
        _follower = GetComponent<UiFollower>();

    private void OnEnable()
    {
        _animator.SetBool(IsHide, false);
        StartCoroutine(HideOverTime());

        _event.AnimatationEnded += ReturnInPool;
    }

    private void OnDisable() =>
        _event.AnimatationEnded -= ReturnInPool;

    public void Follow(Transform target) =>
        _follower.Follow(target);

    public void ReturnInPool() =>
        Deactivated?.Invoke(this);

    private IEnumerator HideOverTime()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_timeVisibleLimits.x, _timeVisibleLimits.y));

        _animator.SetBool(IsHide, true);
    }
}