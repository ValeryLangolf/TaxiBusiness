using UnityEngine;

public class DispatcherPanelShower : MonoBehaviour
{
    private const string IsShow = nameof(IsShow);

    [SerializeField] private Animator _animator;
    [SerializeField] private DispatcherViewToggler _toggler;

    private void OnEnable() =>
        _toggler.Switched += OnSwitch;

    private void OnDisable() =>
        _toggler.Switched -= OnSwitch;

    private void OnSwitch(bool isShow) =>
        _animator.SetBool(IsShow, isShow);
}