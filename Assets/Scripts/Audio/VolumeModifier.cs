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
    [SerializeField] private Slider _sliderVolumeMusic;
    [SerializeField] private Slider _sliderVolumeSound;

    private float _minimumValueSlider;
    private float _maximumValueSlider;

    private void Start()
    {
        _minimumValueSlider = _sliderVolumeMusic.minValue;
        _maximumValueSlider = _sliderVolumeMusic.maxValue;

        OnChangedMusicVolume(_sliderVolumeMusic.value);
        OnChangedGameVolume(_sliderVolumeSound.value);
    }

    private void OnEnable()
    {
        _sliderVolumeMusic.onValueChanged.AddListener(OnChangedMusicVolume);
        _sliderVolumeSound.onValueChanged.AddListener(OnChangedGameVolume);
    }

    private void OnDisable()
    {
        _sliderVolumeMusic.onValueChanged.RemoveListener(OnChangedMusicVolume);
        _sliderVolumeSound.onValueChanged.RemoveListener(OnChangedGameVolume);
    }

    private void OnChangedMusicVolume(float value) =>
        SetLevel(Music, value);

    private void OnChangedGameVolume(float value) =>
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