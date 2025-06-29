using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataService : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Slider _music;
    [SerializeField] private Slider _sound;
    [SerializeField] private DispatcherCenter _dispatcherCenter;
    [SerializeField] private PlayerGarage _garage;

    private readonly Saver _saver = new();
    private Saver.SaveData _data;

    private void Start()
    {
        LoadGameData();
    }

    public void SaveGameData()
    {
        _data.Balance = _wallet.Balance;
        _data.Music = _music.value;
        _data.Sound = _sound.value;
        _data.Dispatchers = GetDispatcherSaveData();
        _data.Vehicles = GetVehicleSaveData();

        _saver.Save(_data);
    }

    private void LoadGameData()
    {
        if (_saver.TryLoad(out _data) == false)
            _data = new Saver.SaveData();

        _wallet.Init(_data.Balance);
        _music.value = _data.Music;
        _sound.value = _data.Sound;
        _dispatcherCenter.Init(_data.Dispatchers);
        _garage.Init(_data.Vehicles);
    }

    public void ResetGame()
    {
        _saver.Save(new Saver.SaveData());
        SfxPlayer.Instance.PlayProgressResetted();
        LoadGameData();
    }

    private List<Saver.DispatcherSaveData> GetDispatcherSaveData()
    {
        List<Saver.DispatcherSaveData> dispatcherSaveDatas = new();

        foreach (DispatcherCard card in _dispatcherCenter.Cards)
            dispatcherSaveDatas.Add(new() { FillAmount = card.Fill });

        return dispatcherSaveDatas;
    }

    private List<Saver.VehicleSaveData> GetVehicleSaveData()
    {
        List<Saver.VehicleSaveData> vehicleSaveDatas = new();

        foreach (VehicleIcon card in _garage.Cards)
            vehicleSaveDatas.Add(new()
            {
                Name = card.Vehicle.Params.Name,
                Position = card.Vehicle.Position,
                Rotation = card.Vehicle.Rotation,
                RemainingFuel = card.Vehicle.Params.RemainingFuel,
                RemainingRepair = card.Vehicle.Params.RemainingRepair,
                IsDriverMan = card.Vehicle.DriverMan != null,
            });

        return vehicleSaveDatas;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveGameData();
    }

    private void OnApplicationQuit() =>
        SaveGameData();
}