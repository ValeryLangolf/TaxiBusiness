using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisingCardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _passengerPercent;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Image _fill;

    public void SetPercent(float percent) =>
        _passengerPercent.SetText($"{percent}%");

    public void SetTime(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        int hours = time.Hours;
        int minutes = time.Minutes;
        int seconds = time.Seconds;

        string formattedTime;

        if (hours > 0)
            formattedTime = string.Format("{0}:{1:D2}:{2:D2}", hours, minutes, seconds);
        else
            formattedTime = string.Format("{0}:{1:D2}", minutes, seconds);

        _time.SetText(formattedTime);
    }

    public void SetPrice(float value) =>
        _price.SetText(value.ToString());

    public void SetFill(float maxValue, float currentValue) =>
        _fill.fillAmount = currentValue/maxValue;

    public void ResetFill() =>
        _fill.fillAmount = 0;
}
