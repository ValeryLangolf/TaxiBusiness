using System;
using UnityEngine;
using UnityEngine.UI;

public class DispatcherViewToggler : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _arrow;
    [SerializeField] private Sprite _leftArrow;
    [SerializeField] private Sprite _rightArrow;

    private bool _isShow = true;

    public event Action<bool> Switched;

    private void Start() =>
        UpdateSprite();

    private void OnEnable() =>
        _button.onClick.AddListener(OnClick);

    private void OnDisable() =>
        _button.onClick.RemoveListener(OnClick);

    private void OnClick()
    {
        _isShow = !_isShow;
        UpdateSprite();
        Switched?.Invoke(_isShow);
    }

    private void UpdateSprite() =>
        _arrow.sprite = _isShow ? _leftArrow : _rightArrow;
}
