using System;
using UnityEngine;
using UnityEngine.UI;

public class DispatcherCard : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _timeInSeconds;
    [SerializeField] private float _salaryRate;

    private float _amount;

    public event Action<DispatcherCard> CycleCompleted;

    public float SalaryRate => _salaryRate;

    private void Update()
    {
        _amount += Time.deltaTime / _timeInSeconds;

        if (_amount >= 1)
        {
            _amount = 0;
            CycleCompleted?.Invoke(this);
        }

        UpdateVisuals();
    }

    private void UpdateVisuals() =>
        _fillImage.fillAmount = _amount;
}