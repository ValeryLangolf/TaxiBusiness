using UnityEngine;
using UnityEngine.UI;

public class SliderVolumeShower : MonoBehaviour
{
    private const string IsShow = nameof(IsShow);

    [SerializeField] private Animator _animator;
    [SerializeField] private Button _button;
    [SerializeField] private Slider _slider;
    [SerializeField] private SliderVolumeShower _other;

    private bool _isShow;

    private void OnEnable() =>
        _button.onClick.AddListener(OnClick);

    private void OnDisable() =>
        _button.onClick.AddListener(OnClick);

    public void Hide()
    {
        _isShow = false;
        _animator.SetBool(IsShow, _isShow);
    }

    private void OnClick()
    {
        _isShow = !_isShow;
        _animator.SetBool(IsShow, _isShow);
        _other.Hide();
    }
}