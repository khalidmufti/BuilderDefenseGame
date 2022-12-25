using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _perlin;
    private float _timer;
    private float _timerMax;
    private float _startingIntensity;

    private void Awake()
    {
        Instance = this;        
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _perlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (_timer < _timerMax)
        {
            _timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(_startingIntensity, 0f, _timer / _timerMax);
            _perlin.m_AmplitudeGain = amplitude;
        }
            
    }

    public void ShakeCamera (float intensity, float maxTimer)
    {
        _timerMax = maxTimer;
        _timer = 0f;
        _startingIntensity = intensity; 
        _perlin.m_AmplitudeGain = intensity;
    }
}
