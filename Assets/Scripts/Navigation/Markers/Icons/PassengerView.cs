using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PassengerAnimationEvent))]
[RequireComponent(typeof(Image))]
public class PassengerView : MonoBehaviour
{
    private const string IsHide = nameof(IsHide);
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _selectedSprite;

    private Vector2 _sizeDefaultIcon;
    private Vector2 _sizeMiniIcon;
    private Animator _animator;
    private PassengerAnimationEvent _event;
    private Image _image;
    private RectTransform _rectTransform;

    public event Action AnimatationEnded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _event = GetComponent<PassengerAnimationEvent>();
        _image = GetComponent<Image>();
        _rectTransform = transform as RectTransform;
        _sizeDefaultIcon = _rectTransform.sizeDelta;
        _sizeMiniIcon = _sizeDefaultIcon * 0.6f;

        _event.AnimatationEnded += AnimatationEnded;
    }

    private void OnEnable()
    {
        _animator.SetBool(IsHide, false);
        Deselect();
    }

    public void AnimateHidding()
    {
        _animator.SetBool(IsHide, true);
        SfxPlayer.Instance.PlayPassengerHiding();
    }

    public void Select() =>
        _image.sprite = _selectedSprite;

    public void Deselect() =>
        _image.sprite = _defaultSprite;

    public void SetMiniIconSize() =>
        _rectTransform.sizeDelta = _sizeMiniIcon;

    public void SetDefaultIconSize()
    {
        if (_rectTransform != null)
            _rectTransform.sizeDelta = _sizeDefaultIcon;
    }
}