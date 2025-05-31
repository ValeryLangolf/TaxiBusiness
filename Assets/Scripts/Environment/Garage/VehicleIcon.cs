using System;
using UnityEngine;
using UnityEngine.UI;

public class VehicleIcon : MonoBehaviour
{
    [SerializeField] private Image _imageIcon;
    [SerializeField] private Image _selectImage;
    [SerializeField] private ButtonClickInformer _clickInformer;

    private Vehicle _vehicle;

    public event Action<VehicleIcon> Clicked;

    public Vehicle Vehicle => _vehicle;

    private void Awake() =>
        Deselect();

    private void OnEnable() =>
        _clickInformer.Clicked += OnClick;

    private void OnDisable() =>
        _clickInformer.Clicked -= OnClick;        

    public void Init(Vehicle vehicle)
    {
        _vehicle = vehicle;
        _imageIcon.sprite = vehicle.Params.Sprite;
    }

    public void Select() =>
        _selectImage.gameObject.SetActive(true);

    public void Deselect() =>
        _selectImage.gameObject.SetActive(false);

    private void OnClick() =>
        Clicked?.Invoke(this);
}