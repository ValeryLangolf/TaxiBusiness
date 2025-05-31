using System;
using UnityEngine;

public class VehicleParams : MonoBehaviour
{
    #region SerializedFields
    [Header("Параметры автомобиля")]

    [Tooltip("Название авто \n(так отображается название в карточке магазина)")]
    [SerializeField]
    private string _name;

    [Tooltip("Краткое описание авто, плюсы и минусы")]
    [SerializeField, TextArea(3, 10)]
    private string _description;

    [Tooltip("Отображается в карточках магазина и иконках авто")]
    [SerializeField] private Sprite _sprite;

    [Tooltip("Скорость авто \n(не соответствует км/ч)")]
    [SerializeField, Range(Constants.MinSpeed, Constants.MaxSpeed)]
    private float _speed;

    [Tooltip("Износостойкость \n(чем выше значение, тем медленнее износ)")]
    [SerializeField, Range(Constants.MinWearResistance, Constants.MaxWearResistance)]
    private float _wearResistance;

    [Tooltip("Экономичность топлива \n(чем выше значение, тем дольше расходуется топливо)")]
    [SerializeField, Range(Constants.MinFuelEfficiency, Constants.MaxFuelEfficiency)]
    private float _fuelEfficiency;

    [Tooltip("Изначальная стоимость авто в магазине \n(может меняться по мере прокачки для последующей продажи)")]
    [SerializeField]
    private float _price;

    [Tooltip("Коэффициент, влияющий на заработок. \n(чем выше значение, тем выше доход с поездки)")]
    [SerializeField, Range(0f, 1f)]
    private float _moneyRate;
    #endregion

    private float _fuelConsumption;
    private float _wearRate;

    public event Action<float> FuelChanged;
    public event Action<float> RepairChanged;
    public event Action<VehicleParams> FuelWasted;
    public event Action<VehicleParams> RepairWasted;
    public event Action<VehicleParams> Destroyed;

    private float _remainingFuel = Constants.FullFuel;
    private float _remainingRepair = Constants.FullRepair;

    #region Properties
    public float MoneyRate => _moneyRate;

    public string Name => _name;

    public Sprite Sprite => _sprite;

    public float Speed => _speed;

    public float WearResistance => _wearResistance;

    public float FuelEfficiency => _fuelEfficiency;

    public string Description => _description;

    public float Price => _price;

    public float RemainingFuel => _remainingFuel;

    public float RemainingRepair => _remainingRepair;

    public bool CanGo => _remainingFuel > 0f && _remainingRepair > 0f;
    #endregion

    private void Awake()
    {
        RecalculateFuelConsumption();
        RecalculateWearResistanceConsumption();
    }

    public void SetRemainingFuel(float value)
    {
        _remainingFuel = value;
        FuelChanged?.Invoke(_remainingFuel);

        if(_remainingFuel == 0)
            FuelWasted?.Invoke(this);
    }

    public bool TryFillFuel(float value)
    {
        float remainingFuel = _remainingFuel + value;

        if (remainingFuel > Constants.FullFuel)
            return false;

        SetRemainingFuel(remainingFuel);
        return true;
    }

    public void BurnFuel()
    {
        if (_remainingFuel == 0)
            return;

        float remainingFuel = Mathf.Max(_remainingFuel - _fuelConsumption * Time.deltaTime, 0f);
        SetRemainingFuel(remainingFuel);
    }

    private void RecalculateFuelConsumption() =>
        _fuelConsumption = Constants.FuelConsumptionMultiplier / _fuelEfficiency;

    public void SetRemainingRepair(float value)
    {
        _remainingRepair = value;
        RepairChanged?.Invoke(_remainingRepair);

        if (_remainingRepair == 0)
            RepairWasted?.Invoke(this);
    }

    public bool TryFillRepair(float value)
    {
        float remainingRepair = _remainingRepair + value;

        if (remainingRepair > Constants.FullRepair)
            return false;

        SetRemainingRepair(remainingRepair);
        return true;
    }

    public void Wear()
    {
        if (_remainingRepair == 0)
            return;

        float remainingRepair = Mathf.Max(_remainingRepair - _wearRate * Time.deltaTime, 0f);
        SetRemainingRepair(remainingRepair);
    }

    private void RecalculateWearResistanceConsumption() =>
        _wearRate = Constants.WearResistanceConsumptionMultiplier / _wearResistance;

    private void OnDestroy() =>
        Destroyed?.Invoke(this);
}