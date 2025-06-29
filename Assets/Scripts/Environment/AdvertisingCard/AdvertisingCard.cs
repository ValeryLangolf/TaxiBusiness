using System;
using UnityEngine;

public class AdvertisingCard : MonoBehaviour
{
    [SerializeField] private AdvertisingCardView _view;
    [SerializeField] private PurchaseButtonHandler _purchasedButton;
    [SerializeField] private float _passengerPercent;
    [SerializeField] private float _timeInSeconds;
    [SerializeField] private float _price;

    private float _remainingTime;
    private bool _isOn;

    public event Action<AdvertisingCard> Started;
    public event Action<AdvertisingCard> Completed;
    public event Action<AdvertisingCard> Clicked;

    public bool IsOn => _isOn;

    public float PassengerPercent => _passengerPercent;

    public float Price => _price;

    private void OnEnable() =>
        _purchasedButton.Clicked += OnClick;

    private void OnDisable() =>
        _purchasedButton.Clicked -= OnClick;

    public void Init()
    {
        ResetParams();
        _view.SetPrice(_price);
    }

    public void ResetParams()
    {
        _view.SetPercent(_passengerPercent);
        _view.SetTime(_timeInSeconds);
        _view.ResetFill();
        _purchasedButton.SetInteractable(true);
        _remainingTime = 0;
        _isOn = false;
    }

    public void UpdateTime()
    {
        if (_isOn == false)
            return;

        _remainingTime -= Time.deltaTime;
        _remainingTime = Mathf.Max(_remainingTime, 0);
        _view.SetFill(_timeInSeconds, _remainingTime);
        _view.SetTime(_remainingTime);

        if (_remainingTime == 0)
        {
            ResetParams();
            Completed?.Invoke(this);
        }
    }

    public void EnableCard()
    {
        _purchasedButton.SetInteractable(false);
        _remainingTime = _timeInSeconds;
        _isOn = true;
        Started?.Invoke(this);
    }

    public void SetInteractButton(bool isOn)
    {
        if(_isOn || isOn == false)
            _purchasedButton.SetInteractable(false);
        else
            _purchasedButton.SetInteractable(true);
    }

    private void OnClick() =>
        Clicked?.Invoke(this);
}