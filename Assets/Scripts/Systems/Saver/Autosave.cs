using System;
using System.Collections;
using UnityEngine;

public class Autosave : MonoBehaviour
{
    private const float SecondsInMinute = 60;

    [SerializeField] private DataService _data;
    [SerializeField] private float _rateInMinutes;

    private WaitForSeconds _wait;

    private void Awake()
    {
        if (_rateInMinutes <= 0)
            throw new ArgumentOutOfRangeException("Число должно быть положительным!");

        _wait = new(_rateInMinutes * SecondsInMinute);
    }

    private void Start() =>
        StartCoroutine(Saving());

    private IEnumerator Saving()
    {
        while (true)
        {
            yield return _wait;

            Save();
        }        
    }

    private void Save() =>
        _data.SaveGameData();
}