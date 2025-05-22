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

    public void Initialize(VehicleConfig config, Action<VehicleShopCard> onClick)
    {
        _clicked = onClick;
        _purchaseButton.Clicked += OnClick;

        if (config == null) 
            return;

        _tittleLabel.text = config.Name;
        _vehicleImage.sprite = config.CarImage;
        _speed.SetValue(config.Speed, Constants.MaxSpeed);
        _strength.SetValue(config.Strength, Constants.MaxStrength);
        _fuel.SetValue(config.Petrol, Constants.MaxPetrol);
        _priceLabel.text = config.Price.ToString("F0");
    }

    private void OnClick() =>
        _clicked?.Invoke(this);
}