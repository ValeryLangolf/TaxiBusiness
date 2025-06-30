using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreferencesShower : MonoBehaviour
{
    private const string IsShow = nameof(IsShow);

    [SerializeField] private VehicleSelector _vehicleSelector;
    [SerializeField] private Button _buttonShowIcons;
    [SerializeField] private Animator _iconPanel;
    [SerializeField] private List<SliderVolumeShower> _sliders;

    private bool _isShow;

    private void Update()
    {
        if(_isShow == false)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            GameObject uiObject = EventSystem.current.currentSelectedGameObject;
            
            if(uiObject != null && uiObject.transform.TryGetComponent(out PreferencesMarker _))
                return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            Hide();
    }

    private void OnEnable() =>
        _buttonShowIcons.onClick.AddListener(OnClickButtonShowIcon);

    private void OnDisable() =>
        _buttonShowIcons.onClick.RemoveListener(OnClickButtonShowIcon);

    private void Hide()
    {
        _isShow = false;
        ProcessSwitch();        
    }

    private void OnClickButtonShowIcon()
    {
        _vehicleSelector.DeselectCurrentVehicle();
        _isShow = !_isShow;
        ProcessSwitch();

        if(_isShow)
            SfxPlayer.Instance.PlaySlidingPanelShow();
        else
            SfxPlayer.Instance.PlaySlidingPanelHide();
    }

    private void ProcessSwitch()
    {
        _iconPanel.SetBool(IsShow, _isShow);

        foreach (SliderVolumeShower slider in _sliders)
            if (slider != null)
                slider.Hide();
    }
}