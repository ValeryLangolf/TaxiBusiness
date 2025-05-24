using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DispatcherCenter : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private PlayerGarage _garage;
    [SerializeField] private PassengerSpawner _passengerSpawner;
    [SerializeField] private TextMeshProUGUI _cardCount;
    [SerializeField] private Button _buttonAdd;
    [SerializeField] private DispatcherCard _prefab;
    [SerializeField] private Transform _content;

    private readonly List<DispatcherCard> _cards = new();

    public event Action<float> Paided;

    private void Awake()
    {
        _cardCount.text = string.Empty;
        ClearAllChildren(_content);

        List<Saver.DispatcherSaveData> dispatcherSaveDatas = Saver.LoadDispatchers();

        foreach (Saver.DispatcherSaveData data in dispatcherSaveDatas)
            InstantiateCard(data.FillAmount);
    }

    private void ClearAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
            Destroy(child.gameObject);
    }

    private void OnEnable() =>
        _buttonAdd.onClick.AddListener(OnClickAdd);

    private void OnDisable() =>
        _buttonAdd.onClick.RemoveListener(OnClickAdd);

    private void OnClickAdd() =>
        InstantiateCard(0);

    private void InstantiateCard(float fillAmount)
    {
        DispatcherCard card = Instantiate(_prefab, _content);
        Subscribe(card);
        card.SetFill(fillAmount);
    }

    private void Subscribe(DispatcherCard card)
    {
        _cards.Add(card);
        _cardCount.text = _cards.Count.ToString();

        card.CycleCompleted += OnCardCycleCompleted;
        card.RemoveClicked += OnRemoveClick;
    }

    private void Unsubscribe(DispatcherCard card)
    {
        card.CycleCompleted -= OnCardCycleCompleted;
        card.RemoveClicked -= OnRemoveClick;

        _cards.Remove(card);
        _cardCount.text = _cards.Count.ToString();
    }

    private void OnCardCycleCompleted(DispatcherCard card)
    {
        _wallet.EmptyWallet(card.SalaryRate);
        AssignPassengerVehicle();

        Paided?.Invoke(card.SalaryRate);
    }

    private void OnRemoveClick(DispatcherCard card)
    {
        Unsubscribe(card);
        Destroy(card.gameObject);
    }

    private void AssignPassengerVehicle()
    {
        if (TryGetRandomVehicle(out Vehicle vehicle) == false)
            return;

        if (TryGetRandomPassenger(out Passenger passenger) == false)
            return;

        vehicle.SetPassenger(passenger);
        vehicle.SetDestination(passenger.Target.position);
    }

    private bool TryGetRandomVehicle(out Vehicle vehicle)
    {
        vehicle = null;
        List<VehicleParams> vehicles = _garage.GetAvailable();

        if (vehicles.Count == 0)
            return false;

        int randomId = UnityEngine.Random.Range(0, vehicles.Count);
        vehicle = vehicles[randomId].Vehicle;

        return true;
    }

    private bool TryGetRandomPassenger(out Passenger passenger)
    {
        passenger = null;

        List<Passenger> passengers = _passengerSpawner.GetAwailable();

        if (passengers.Count == 0)
            return false;

        int randomId = UnityEngine.Random.Range(0, passengers.Count);
        passenger = passengers[randomId];

        return true;
    }

    private void SaveGameData() =>
        Saver.SaveDispatcher(_cards);

    private void OnApplicationQuit() =>
        SaveGameData();
}