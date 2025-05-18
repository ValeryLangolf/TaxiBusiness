using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconDestinationShowerView : MonoBehaviour
{
    [SerializeField] private Sprite _iconNoPassanger;
    [SerializeField] private Sprite _iconPassangerTransportation;

    private Image _image;

    private void Awake() =>
        _image = GetComponent<Image>();

    public void SetPassengerFinishIcon() =>
        _image.sprite = _iconPassangerTransportation;

    public void SetDefaultIcon() =>
        _image.sprite = _iconNoPassanger;

    public void Show() =>
        gameObject.SetActive(true);

    public void Hide() =>
        gameObject.SetActive(false);
}