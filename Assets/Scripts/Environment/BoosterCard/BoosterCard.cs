using System;
using UnityEngine;

public class BoosterCard : MonoBehaviour
{
    [SerializeField] private BoosterCardView _view;
    [SerializeField] private PurchaseButtonHandler _purchasedButton;
    [SerializeField] private BoosterType _type;
    [SerializeField] private RotatotImageRepeatButton _repeatButton;
    [SerializeField] private float _passengerPercent;
    [SerializeField] private float _timeInSeconds;
    [SerializeField] private float _price;

    private float _remainingTime;
    private bool _isOn;

    public event Action<BoosterCard> Started;
    public event Action<BoosterCard> Completed;
    public event Action<BoosterCard> Clicked;

    public bool IsOn => _isOn;

    public float PassengerPercent => _passengerPercent;

    public float Price => _price;

    public bool IsRepeat => _repeatButton.IsPlaying;

    public BoosterType Type => _type;

    private void OnEnable() =>
        _purchasedButton.Clicked += OnClickPurchasedButton;

    private void OnDisable() =>
        _purchasedButton.Clicked -= OnClickPurchasedButton;

    public void Init()
    {
        ResetParams();
        _view.SetPercent(_passengerPercent);
        _view.SetPrice(_price);
    }

    public void UpdateTime()
    {
        if (_isOn == false)
            return;

        _remainingTime -= Time.deltaTime;
        _remainingTime = Mathf.Max(_remainingTime, 0);
        _view.SetFill(_remainingTime/_timeInSeconds);
        _view.SetTime(_remainingTime);

        if (_remainingTime == 0)
        {
            ResetParams();
            Completed?.Invoke(this);
        }
    }

    public void Enable()
    {
        _remainingTime = _timeInSeconds;
        _isOn = true;
        Started?.Invoke(this);
    }

    public void SetInteractButton(bool canOn)
    {
        if(_isOn || canOn == false)
            _purchasedButton.SetInteractable(false);
        else
            _purchasedButton.SetInteractable(true);
    }

    private void OnClickPurchasedButton() =>
        Clicked?.Invoke(this);

    private void ResetParams()
    {
        _view.SetTime(_timeInSeconds);
        _view.ResetFill();        
        _remainingTime = 0;
        _isOn = false;
    }
}