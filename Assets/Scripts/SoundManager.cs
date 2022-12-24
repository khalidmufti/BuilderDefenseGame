using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
    }

    private AudioSource _audioSource;
    private Dictionary<Sound, AudioClip> _soundAudioClipDictionary;
    private float _volume = 0.5f;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            _soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());    
        }
    }

    public void PlaySound(Sound sound)
    {
        if (sound == Sound.GameOver)
        {
            _audioSource.Stop();
        }
        _audioSource.PlayOneShot(_soundAudioClipDictionary[sound], _volume);
    }

    public void IncreaseVolume ()
    {
        _volume += 0.1f;
        _volume = Mathf.Clamp01(_volume);
    }

    public void DecreaseVolume()
    {
        _volume -= 0.1f;
        _volume = Mathf.Clamp01(_volume);
    }

    public float GetVolume ()
    {
        return _volume;
    }
}
