using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleShopCard : MonoBehaviour
{
    #region Fields
    [SerializeField] private TextMeshProUGUI _tittleLabel;
    [SerializeField] private Image _vehicleImage;
    [SerializeField] private ShopCardSlider _speed;
    [SerializeField] private ShopCardSlider _strength;
    [SerializeField] private ShopCardSlider _fuel;
    [SerializeField] private TextMeshProUGUI _priceLabel;
    [SerializeField] private PurchaseButtonHandler _purchaseButton;
    #endregion

    private Action<VehicleShopCard> _clicked;

    private void OnDestroy() =>
        _purchaseButton.Clicked -= OnClick;

    public void Initialize(Vehicle vehicle, Action<VehicleShopCard> onClick)
    {
        _clicked = onClick;
        _purchaseButton.Clicked += OnClick;

        if (vehicle == null) 
            return;

        _tittleLabel.text = vehicle.Name;
        _vehicleImage.sprite = vehicle.Sprite;
        _speed.SetValue(vehicle.Speed, Constants.MaxSpeed);
        _strength.SetValue(vehicle.WearResistance, Constants.MaxWearResistance);
        _fuel.SetValue(vehicle.FuelEfficiency, Constants.MaxFuelEfficiency);
        _priceLabel.text = vehicle.Price.ToString("F0");
    }

    public void SetInteractButton(bool isOn) =>
        _purchaseButton.SetInteractable(isOn);

    private void OnClick() =>
        _clicked?.Invoke(this);
}