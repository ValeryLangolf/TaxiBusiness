using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleShopCard : MonoBehaviour
{
    #region Fields
    [SerializeField] private Image _rating;
    [SerializeField] private TextMeshProUGUI _tittleLabel;
    [SerializeField] private Image _vehicleImage;
    [SerializeField] private ShopCardSlider _speed;
    [SerializeField] private ShopCardSlider _strength;
    [SerializeField] private ShopCardSlider _fuel;
    [SerializeField] private TextMeshProUGUI _descriptionLabel;
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

        VehicleParams vehicleParams = vehicle.Params;

        _rating.fillAmount = Mathf.Clamp(vehicleParams.MoneyRate, 0, 1);
        _tittleLabel.text = vehicleParams.Name;
        _vehicleImage.sprite = vehicleParams.Sprite;
        _speed.SetValue(vehicleParams.Speed, Constants.MaxSpeed);
        _strength.SetValue(vehicleParams.WearResistance, Constants.MaxWearResistance);
        _fuel.SetValue(vehicleParams.FuelEfficiency, Constants.MaxFuelEfficiency);
        _descriptionLabel.text = vehicleParams.Description;
        _priceLabel.text = vehicleParams.Price.ToString("F0");
    }

    public void SetInteractButton(bool isOn) =>
        _purchaseButton.SetInteractable(isOn);

    private void OnClick() =>
        _clicked?.Invoke(this);
}