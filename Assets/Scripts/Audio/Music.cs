using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private List<AudioClip> _clips;
    [SerializeField] private bool _isRandomSequence;

    private int _currentClipIndex = -1;

    private void Start() =>
        StartCoroutine(SwitchOverTime());

    private void PlayNextClip()
    {
        _source.clip = GetClip();
        _source.Play();
    }

    private AudioClip GetClip()
    {
        _currentClipIndex = GetNextIndex();

        return _clips[_currentClipIndex];
    }

    private int GetNextIndex()
    {
        if (_isRandomSequence)
            return Random.Range(0, _clips.Count);
        else
            return (_currentClipIndex + 1) % _clips.Count;
    }

    private IEnumerator SwitchOverTime()
    {
        while (true)
        {
            PlayNextClip();

            yield return new WaitForSeconds(_source.clip.length);
        }
    }
}