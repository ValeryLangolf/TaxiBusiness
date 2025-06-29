using UnityEngine;
using UnityEngine.UI;

public class RotatotImageRepeatButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _image;
    [SerializeField] private Vector3 _rotationPerSecond;

    private bool _isPlaying;

    public bool IsPlaying => _isPlaying;

    private void Update()
    {
        if(_isPlaying == false)
            return;

        _image.localEulerAngles += _rotationPerSecond * Time.deltaTime;
    }

    private void OnEnable() =>
        _button.onClick.AddListener(OnClick);

    private void OnDisable() =>
        _button.onClick.RemoveListener(OnClick);

    private void OnClick()
    {
        _isPlaying = !_isPlaying;

        if(_isPlaying == false)
            _image.localEulerAngles = Vector3.zero;
    }
}