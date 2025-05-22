using UnityEngine;

public class CarShowroomShower : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private ButtonClickInformer _icon;
    [SerializeField] private ButtonClickInformer _closeButton;

    private void Awake() =>
        _panel.SetActive(false);

    private void OnEnable()
    {
        _icon.Clicked += OnClickIcon;
        _closeButton.Clicked += OnClickClose;
    }

    private void OnDisable()
    {
        _icon.Clicked -= OnClickIcon;
        _closeButton.Clicked -= OnClickClose;
    }

    private void OnClickIcon() =>
        _panel.SetActive(true);

    private void OnClickClose() =>
        _panel.SetActive(false);
}