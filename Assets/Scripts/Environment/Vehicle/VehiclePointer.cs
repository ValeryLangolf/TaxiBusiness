using UnityEngine;

public class VehiclePointer : MonoBehaviour
{
    [SerializeField] private VehicleSelector _selector;
    [SerializeField] private GameObject _point;
    [SerializeField] private GameObject _particleContent;
    [SerializeField] float _speedRotation;

    private Transform _target;

    private void Awake()
    {
        _selector.Selected += OnSelected;
        _selector.Deselected += OnDeselected;

        OnDeselected(null);
    }

    private void OnDestroy()
    {
        _selector.Selected -= OnSelected;
        _selector.Deselected -= OnDeselected;
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        transform.position = _target.position;
        transform.localRotation *= Quaternion.Euler(0, _speedRotation * Time.deltaTime, 0);
    }

    public void OnSelected(Vehicle target)
    {
        _point.SetActive(true);
        _particleContent.SetActive(true);
        _target = target.transform;
    }

    public void OnDeselected(Vehicle _)
    {
        _point.SetActive(false);
        _particleContent.SetActive(false);
        _target = null;
    }
}