using UnityEngine;

public class PaymentsDispatcherIndicator : MonoBehaviour
{
    [SerializeField] private Expenses _profitViewPrefab;
    [SerializeField] private DispatcherCenter _dispatcherCenter;

    private Pool<Expenses> _pool;

    private void Awake() =>
        _pool = new(_profitViewPrefab, transform);

    private void OnEnable() =>
        _dispatcherCenter.Paided += OnPaided;

    private void OnDisable() =>
        _dispatcherCenter.Paided -= OnPaided;

    private void OnPaided(float value)
    {
        if (_pool.TryGet(out Expenses profitView) == false)
            return;

        profitView.SetText(value);
        profitView.transform.position = transform.position;
    }
}