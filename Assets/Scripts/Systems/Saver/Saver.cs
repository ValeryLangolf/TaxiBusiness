using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Saver
{
    private const string DataFile = "DataFile.json";

    [Serializable]
    public class SaveData
    {
        public float Balance = Constants.InitialBalance;
        public float Music = Constants.InitialVolumeMusic;
        public float Sound = Constants.InitialVolumeSound;
        public List<DispatcherSaveData> Dispatchers = new();
        public List<VehicleSaveData> Vehicles = new();
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
        public float RemainingFuel;
        public float RemainingRepair;
        public bool IsDriverMan;
    }

    public bool TryLoad(out SaveData saveData)
    {
        saveData = null;

        if (File.Exists(GetPath(DataFile)) == false)
            return false;

        try
        {
            string json = File.ReadAllText(GetPath(DataFile));
            saveData = JsonUtility.FromJson<SaveData>(json);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Не удалось загрузить файлы сохранений: {e.Message}");
            return false;
        }
    }

    public void Save(SaveData saveData)
    {
        try
        {
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(GetPath(DataFile), json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Не удалось сохранить: {e.Message}");
        }
    }

    private string GetPath(string fileName) =>
        Path.Combine(Application.persistentDataPath, fileName);
}