using UnityEngine;
using UnityEngine.UI;

public class ShopCardSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void SetValue(float currentValue, float maxValue)
    {
        float normalizedValue = Mathf.Clamp01(currentValue / maxValue);
        _slider.value = normalizedValue * _slider.maxValue;
    }
}