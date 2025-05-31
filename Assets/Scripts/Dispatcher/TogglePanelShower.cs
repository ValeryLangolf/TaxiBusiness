using UnityEngine;

public class TogglePanelShower : MonoBehaviour
{
    private const string IsShow = nameof(IsShow);

    [SerializeField] private Animator _animator;
    [SerializeField] private Toggler _toggler;

    private void OnEnable() =>
        _toggler.Switched += OnSwitch;

    private void OnDisable() =>
        _toggler.Switched -= OnSwitch;

    private void OnSwitch(bool isShow) =>
        _animator.SetBool(IsShow, isShow);
}