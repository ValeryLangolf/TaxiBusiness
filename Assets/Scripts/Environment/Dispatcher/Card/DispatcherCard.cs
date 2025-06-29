using System;
using UnityEngine;
using UnityEngine.UI;

public class DispatcherCard : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Button _buttonRemove;
    [SerializeField] private float _timeInSeconds;
    [SerializeField] private float _salaryRate;

    private float _amount;

    public event Action<DispatcherCard> CycleCompleted;
    public event Action<DispatcherCard> RemoveClicked;

    public float Fill => _amount;

    public float SalaryRate => _salaryRate;

    private void OnEnable() =>
        _buttonRemove.onClick.AddListener(OnClickRemove);

    private void OnDisable() =>
        _buttonRemove.onClick.RemoveListener(OnClickRemove);

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

    public void SetFill(float value) =>
        _amount = value;

    private void OnClickRemove() =>
        RemoveClicked?.Invoke(this);

    private void UpdateVisuals() =>
        _fillImage.fillAmount = _amount;
}