using System;
using UnityEngine;
using UnityEngine.UI;

public class VehicleIcon : MonoBehaviour
{
    [SerializeField] private Image _imageIcon;
    [SerializeField] private Image _selectImage;
    [SerializeField] private ButtonClickInformer _clickInformer;

    public event Action<VehicleIcon> Clicked;

    private void Awake() =>
        Deselect();

    private void OnEnable() =>
        _clickInformer.Clicked += OnClick;

    private void OnDisable() =>
        _clickInformer.Clicked -= OnClick;

    public void SetIcon(Sprite sprite) =>
        _imageIcon.sprite = sprite;

    public void Select() =>
        _selectImage.gameObject.SetActive(true);

    public void Deselect() =>
        _selectImage.gameObject.SetActive(false);

    private void OnClick() =>
        Clicked?.Invoke(this);
}