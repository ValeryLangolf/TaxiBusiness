using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UiFollower : MonoBehaviour
{
    private Transform _target;
    private RectTransform _self;
    private RectTransform _parent;
    private Canvas _canvas;
    private Camera _camera;

    public Transform Target => _target;

    private void Awake()
    {
        _self = transform as RectTransform;
        _parent = _self.parent as RectTransform;
        _canvas = GetComponentsInParent<Canvas>()[0];
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_target == null)
            return;

        Vector3 screenPosition = _camera.WorldToScreenPoint(_target.position);

        bool isBehindCamera = screenPosition.z < 0;
        gameObject.SetActive(isBehindCamera == false);

        if (isBehindCamera)
            return;

        UpdateUiPositionFromWorldTarget(screenPosition);
    }

    public void Follow(Transform target)
    {
        if (target == null)
            throw new ArgumentNullException("Таргет не был установлен");

        _target = target;
    }

    public void Stop() =>
        _target = null;

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