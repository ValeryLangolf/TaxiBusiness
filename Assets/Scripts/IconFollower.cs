using Unity.VisualScripting;
using UnityEngine;

public class IconFollower : MonoBehaviour
{
    private Transform _target;
    private RectTransform _self;
    private Canvas _canvas;

    private void Awake()
    {
        _self = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (_target == null)
            return;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_target.position);

        if (screenPosition.z < 0)
        {
            _self.gameObject.SetActive(false);
            return;
        }
        else
        {
            _self.gameObject.SetActive(true);
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _self.parent as RectTransform,
            screenPosition,
            _canvas.worldCamera,
            out Vector2 localPoint
        );

        _self.anchoredPosition = localPoint;
    }

    public void Follow(Transform target)
    {
        _target = target;
        gameObject.SetActive(true);
    }

    public void Stop()
    {
        _target = null;
        gameObject.SetActive(false);
    }
}