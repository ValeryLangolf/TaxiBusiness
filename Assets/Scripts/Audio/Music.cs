using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private List<AudioClip> _clips;
    [SerializeField] private bool _isRandomSequence;

    private int _currentClipIndex;

    private void Start() =>
        PlayNextClip();

    private void Update()
    {
        if (!_source.isPlaying)
            PlayNextClip();
    }

    private void PlayNextClip()
    {
        _source.clip = GetClip();
        _source.Play();
    }

    private AudioClip GetClip()
    {
        if (_isRandomSequence)
            _currentClipIndex = Random.Range(0, _clips.Count);
        else
            _currentClipIndex = (_currentClipIndex + 1) % _clips.Count;

        return _clips[_currentClipIndex];
    }
}