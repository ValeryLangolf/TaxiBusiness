using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UiFollower))]
public class Passenger : MonoBehaviour, IDeactivatable<Passenger>
{
    [SerializeField] private PassengerView _view;
    [SerializeField] private Vector2 _timeVisibleLimits;

    private Waypoint _destination;
    private Transform _passengerPoint;
    private UiFollower _follower;
    private Coroutine _coroutine;

    public event Action<Passenger> Deactivated;
    public event Action<Passenger> Refused;

    public Transform Point => _passengerPoint;

    public Waypoint Destination => _destination;

    private void Awake()
    {
        _follower = GetComponent<UiFollower>();
        _view.AnimatationEnded += OnAnimationEnded;
    }

    private void OnEnable()
    {
        _view.SetDefaultIconSize();
        _coroutine = StartCoroutine(HideOverTime());
        SfxPlayer.Instance.PlayPassengerShowing();
    }

    public void Follow(Transform target)
    {
        _passengerPoint = target;
        _follower.Follow(target.transform);
    }

    public void PickUp(Transform taxi)
    {
        if (this.enabled)
            StopCoroutine(_coroutine);

        _view.SetMiniIconSize();
        Follow(taxi);
    }

    public void SetDestination(Waypoint point) =>
        _destination = point;

    public void Select() =>
        _view.Select();

    public void Deselect() =>
        _view.Deselect();

    public void ReturnInPool() =>
        Deactivated?.Invoke(this);

    private IEnumerator HideOverTime()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_timeVisibleLimits.x, _timeVisibleLimits.y));

        _view.AnimateHidding();
        Refused?.Invoke(this);
    }

    private void OnAnimationEnded() =>
        ReturnInPool();        
}