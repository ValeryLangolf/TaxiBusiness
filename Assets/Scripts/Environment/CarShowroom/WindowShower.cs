using UnityEngine;

public class WindowShower : MonoBehaviour
{
    [SerializeField] private VehicleSelector _vehicleSelector;
    [SerializeField] private MouseHitInformer _mouseHitInformer;
    [SerializeField] private GameObject _panel;
    [SerializeField] private ButtonClickInformer _icon;
    [SerializeField] private ButtonClickInformer _closeButton;

    private void Awake() =>
        _panel.SetActive(false);

    private void OnEnable()
    {
        _icon.Clicked += OnClickIcon;
        _closeButton.Clicked += OnClickClose;
        _mouseHitInformer.WorldClicked += OnClickClose;
    }

    private void OnDisable()
    {
        _icon.Clicked -= OnClickIcon;
        _closeButton.Clicked -= OnClickClose;
        _mouseHitInformer.WorldClicked -= OnClickClose;
    }

    private void OnClickIcon()
    {
        _vehicleSelector.DeselectCurrentVehicle();
        _panel.SetActive(true);

        SfxPlayer.Instance.PlayPopUpPanelShow();
    }

    private void OnClickClose()
    {
        if (_panel.gameObject.activeInHierarchy == false)
            return;

        _panel.SetActive(false);
        SfxPlayer.Instance.PlayPopUpPanelHide();
    }
}