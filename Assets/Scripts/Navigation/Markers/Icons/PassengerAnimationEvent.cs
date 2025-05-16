using System;
using UnityEngine;

public class PassengerAnimationEvent : MonoBehaviour
{
    public event Action AnimatationEnded;

    public void HideIcon() =>
        AnimatationEnded?.Invoke();
}