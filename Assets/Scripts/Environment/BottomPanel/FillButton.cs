using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AudioSource _source;

    private bool _isPressed;

    public event Action Pressed;
    public event Action Unpressed;

    public bool IsPressed => _isPressed;

    public bool IPlaying => _source.isPlaying;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        Pressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        Unpressed?.Invoke();
    }

    public void StopSound() =>
        _source.Stop();

    public void PlaySound() =>
        _source.Play();
}