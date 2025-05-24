using UnityEngine;

public class ProfitShower : MonoBehaviour
{
    [SerializeField] private ProfitView _profitViewPrefab;
    [SerializeField] private PlayerGarage _garage;

    private Pool<ProfitView> _pool;

    private void Awake() =>
        _pool = new(_profitViewPrefab, transform);

    private void OnEnable() =>
        _garage.MoneyAdded += OnMoneyAdded;

    private void OnDisable() =>
        _garage.MoneyAdded -= OnMoneyAdded;

    private void OnMoneyAdded(float profit, Vector3 position)
    {
        if (_pool.TryGet(out ProfitView profitView) == false)
            return;

        profitView.SetText(profit);
        profitView.SetPositionUi(position);
    }
}