using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Saver
{
    private const string BalanceFileName = "balance_save.json";
    private const string DispatchersFileName = "dispatcher_save.json";
    private const string VehiclesFileName = "vehicles_save.json";

    [Serializable]
    public class BalanceSaveData
    {
        public float PlayerBalance;
    }

    [Serializable]
    public class DispatcherSaveData
    {
        public float FillAmount;
    }

    [Serializable]
    public class VehicleSaveData
    {
        public string Name;
        public Vector3 Position;
        public Quaternion Rotation;
    }

    [Serializable]
    public class VehiclesList
    {
        public List<VehicleSaveData> Vehicles = new();
    }

    [Serializable]
    public class DispatchersList
    {
        public List<DispatcherSaveData> Dispatchers = new();
    }

    public static void SaveBalance(float balance)
    {
        var balanceData = new BalanceSaveData { PlayerBalance = balance };
        SaveData(GetPath(BalanceFileName), balanceData);
    }

    public static void SaveDispatcher(List<DispatcherCard> dispatcherCards)
    {
        DispatchersList dispatchersData = new () 
        { 
            Dispatchers = dispatcherCards.Select(d => new DispatcherSaveData 
            { 
                FillAmount = d.Fill 
            }).ToList()
        };

        SaveData(GetPath(DispatchersFileName), dispatchersData);
    }

    public static void SaveVehicles(List<Vehicle> vehicles)
    {
        VehiclesList vehiclesData = new ()
        {
            Vehicles = vehicles.Select(vehicle => new VehicleSaveData
            {
                Name = vehicle.Name,
                Position = vehicle.Position,
                Rotation = vehicle.Rotation
            }).ToList()
        };

        SaveData(GetPath(VehiclesFileName), vehiclesData);
    }

    private static void SaveData<T>(string path, T data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    public static float LoadBalance()
    {
        if (File.Exists(GetPath(BalanceFileName)) == false) 
            return Constants.InitialBalance;

        try
        {
            string json = File.ReadAllText(GetPath(BalanceFileName));
            var balanceData = JsonUtility.FromJson<BalanceSaveData>(json);
            return balanceData.PlayerBalance;
        }
        catch (Exception e)
        {
            Debug.LogError($"Load balance failed: {e.Message}");
            return 0;
        }
    }

    public static List<DispatcherSaveData> LoadDispatchers()
    {
        if (File.Exists(GetPath(DispatchersFileName)) == false)
            return new();

        try
        {
            string json = File.ReadAllText(GetPath(DispatchersFileName));
            DispatchersList dispathersData = JsonUtility.FromJson<DispatchersList>(json);
            return dispathersData.Dispatchers;
        }
        catch (Exception e)
        {
            Debug.LogError($"Load dispatchers failed: {e.Message}");
            return new();
        }
    }

    public static List<VehicleSaveData> LoadVehicles()
    {
        if (File.Exists(GetPath(VehiclesFileName)) == false) 
            return new();

        try
        {
            string json = File.ReadAllText(GetPath(VehiclesFileName));
            var vehiclesData = JsonUtility.FromJson<VehiclesList>(json);
            return vehiclesData.Vehicles;
        }
        catch (Exception e)
        {
            Debug.LogError($"Load vehicles failed: {e.Message}");
            return new();
        }
    }

    private static string GetPath(string fileName) =>
        Path.Combine(Application.persistentDataPath, fileName);
}