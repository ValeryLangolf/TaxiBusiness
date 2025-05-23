using System;
using UnityEngine;
using UnityEngine.UI;

public class GarageViewToggler : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _arrow;
    [SerializeField] private Sprite _leftArrow;
    [SerializeField] private Sprite _rightArrow;

    private bool _isShow;

    public event Action<bool> Switched;

    public bool IsShow => _isShow;

    private void Awake() =>
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
        _arrow.sprite = _isShow ? _rightArrow : _leftArrow;
}