using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonStatusPressedInformer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action Pressed;
    public event Action Unpressed;

    public void OnPointerDown(PointerEventData eventData) =>
        Pressed?.Invoke();

    public void OnPointerUp(PointerEventData eventData) =>
        Unpressed?.Invoke();
}