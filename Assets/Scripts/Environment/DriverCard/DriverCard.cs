using System;
using UnityEngine;
using UnityEngine.UI;

public class DriverCard : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private ButtonClickInformer _clickInformer;

    private Sprite _defaultSprite;

    public event Action Clicked;

    private void Awake() =>
        _defaultSprite = _image.sprite;

    private void OnEnable() =>
        _clickInformer.Clicked += OnClick;

    private void OnDisable() =>
        _clickInformer.Clicked -= OnClick;

    public void SetSprite(Sprite sprite) =>
        _image.sprite = sprite;

    public void ResetSprite() =>
        _image.sprite = _defaultSprite;

    private void OnClick() =>
        Clicked?.Invoke();
}