using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleSaleCard : MonoBehaviour
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
    [SerializeField] private PurchaseButtonHandler _sellButton;
    #endregion

    private Action<VehicleSaleCard> _clicked;

    private void OnDestroy() =>
        _sellButton.Clicked -= OnClick;

    public void Initialize(Vehicle vehicle, float salesIncome, Action<VehicleSaleCard> onClick)
    {
        _clicked = onClick;
        _sellButton.Clicked += OnClick;

        if (vehicle == null)
            return;

        _rating.fillAmount = Mathf.Clamp(vehicle.MoneyRate, 0, 1);
        _tittleLabel.text = vehicle.Name;
        _vehicleImage.sprite = vehicle.Sprite;
        _speed.SetValue(vehicle.Speed, Constants.MaxSpeed);
        _strength.SetValue(vehicle.WearResistance, Constants.MaxWearResistance);
        _fuel.SetValue(vehicle.FuelEfficiency, Constants.MaxFuelEfficiency);
        _descriptionLabel.text = vehicle.Description;
        _priceLabel.text = (vehicle.Price * salesIncome).ToString("F0");
    }

    public void SetInteractButton(bool isOn) =>
        _sellButton.SetInteractable(isOn);

    private void OnClick() =>
        _clicked?.Invoke(this);
}