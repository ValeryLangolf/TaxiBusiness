using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestinationPointClickMarker : MonoBehaviour, IDeactivatable<DestinationPointClickMarker>
{
    private Animator _animator;

    public event Action<DestinationPointClickMarker> Deactivated;

    private void Awake() =>
        _animator = GetComponent<Animator>();

    private void OnEnable() =>
        StartCoroutine(WaitAnimationEnd());

    public void SetPosition(Vector3 position) =>
        transform.position = position;

    public void ReturnInPool() =>
        Deactivated?.Invoke(this);

    private IEnumerator WaitAnimationEnd()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        ReturnInPool();
    }
}