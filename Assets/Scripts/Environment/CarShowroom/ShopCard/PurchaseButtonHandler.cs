using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButtonHandler : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Color _activeTextColor;
    [SerializeField] private Color _inactiveTextColor;

    public event Action Clicked;

    private void OnEnable() =>
        _button.onClick.AddListener(OnClick);

    private void OnDisable() =>
        _button.onClick.RemoveListener(OnClick);

    public void SetInteractable(bool isInteractable)
    {
        _button.interactable = isInteractable;
        _buttonText.color = isInteractable ? _activeTextColor : _inactiveTextColor;
    }

    private void OnClick() =>
        Clicked?.Invoke();
}