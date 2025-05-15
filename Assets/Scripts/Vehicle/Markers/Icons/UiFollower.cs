using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class UiFollower : MonoBehaviour
{
    private Image _image;
    private Transform _target;
    private RectTransform _self;
    private RectTransform _parent;
    private Canvas _canvas;
    private Camera _camera;

    private void Awake()
    {
        _image = GetComponent<Image>();
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
        _image.enabled = isBehindCamera == false;

        if (isBehindCamera)
            return;

        UpdateUiPositionFromWorldTarget(screenPosition);
    }

    public void Follow(Transform target) =>
        _target = target;

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