using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeModifier : MonoBehaviour
{
    private const float MinimumLevel = -80;
    private const float MaximumLevel = 20;

    private const string Music = nameof(Music);
    private const string Sound = nameof(Sound);

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;

    private float _minimumValueSlider;
    private float _maximumValueSlider;

    private void Start()
    {
        _minimumValueSlider = _musicSlider.minValue;
        _maximumValueSlider = _musicSlider.maxValue;

        OnChangedMusic(_musicSlider.value);
        OnChangedSound(_soundSlider.value);
    }

    private void OnEnable()
    {
        _musicSlider.onValueChanged.AddListener(OnChangedMusic);
        _soundSlider.onValueChanged.AddListener(OnChangedSound);
    }

    private void OnDisable()
    {
        _musicSlider.onValueChanged.RemoveListener(OnChangedMusic);
        _soundSlider.onValueChanged.RemoveListener(OnChangedSound);
    }

    private void OnChangedMusic(float value) =>
        SetLevel(Music, value);

    private void OnChangedSound(float value) =>
        SetLevel(Sound, value);

    public void SetLevel(string group, float value)
    {
        float level = ConvertVolumeToLevel(NormalizeValue(value));
        _mixer.SetFloat(group, level);
    }

    private float NormalizeValue(float value) =>
        Mathf.InverseLerp(_minimumValueSlider, _maximumValueSlider, value);

    private float ConvertVolumeToLevel(float value) =>
        value == 0 ? MinimumLevel : Mathf.Log10(value) * MaximumLevel;
}