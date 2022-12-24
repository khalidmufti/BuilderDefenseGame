using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource _audioSource;
    private float _volume = 0.5f;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _volume;
    }

    public void IncreaseVolume()
    {
        _volume += 0.1f;
        _volume = Mathf.Clamp01(_volume);
        _audioSource.volume = _volume;
    }

    public void DecreaseVolume()
    {
        _volume -= 0.1f;
        _volume = Mathf.Clamp01(_volume);
        _audioSource.volume = _volume;
    }

    public float GetVolume()
    {
        return _volume;
    }
}
