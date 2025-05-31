using UnityEngine;
using UnityEngine.UI;

public class BottomPanelView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _remainingFuelFillImage;
    [SerializeField] private Image _remainingRepairFillImage;

    private Sprite _initialSprite;

    private void Awake() =>
        _initialSprite = _image.sprite;

    public void ResetSprite() =>
        _image.sprite = _initialSprite;

    public void SetSprite(Sprite vehicleSprite) =>
        _image.sprite = vehicleSprite;

    public void SetRemainingFuel(float value) =>
        _remainingFuelFillImage.fillAmount = value;

    public void SetRemainingRepair(float value) =>
        _remainingRepairFillImage.fillAmount = value;
}