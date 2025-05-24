using System;
using TMPro;
using UnityEngine;

public class Expenses : MonoBehaviour, IDeactivatable<Expenses>
{
    [SerializeField] private TextMeshProUGUI _profitText;

    private RectTransform _self;
    private RectTransform _parent;
    private Canvas _canvas;
    private Camera _camera;

    public event Action<Expenses> Deactivated;

    private void Awake()
    {
        _self = transform as RectTransform;
        _parent = _self.parent as RectTransform;
        _canvas = GetComponentsInParent<Canvas>()[0];
        _camera = Camera.main;
    }

    public void ReturnInPool() =>
        Deactivated?.Invoke(this);

    public void SetText(float value)
    {
        _profitText.text = $"-{value:F0}";
        Invoke(nameof(ReturnInPool), 2f);
    }

    public void SetPositionUi(Vector3 target)
    {
        Vector3 screenPosition = _camera.WorldToScreenPoint(target);
        UpdateUiPositionFromWorldTarget(screenPosition);
    }

    private void UpdateUiPositionFromWorldTarget(Vector3 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parent,
            screenPosition,
            _canvas.worldCamera,
            out Vector2 localPoint
        );

        _self.anchoredPosition = localPoint;
    }
}