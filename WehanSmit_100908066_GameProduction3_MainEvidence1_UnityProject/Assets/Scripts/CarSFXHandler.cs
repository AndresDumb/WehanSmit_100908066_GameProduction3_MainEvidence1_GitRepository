using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSFXHandler : MonoBehaviour
{
    private RaceCarController _raceCarController;
    public AudioSource _tireScreechingAudioSource;
    public AudioSource _engineAudioSource;
    public AudioSource _carHitAudioSource;
    private float tireScreechPitch = 0.5f;

    private float desiredEnginePitch = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        _raceCarController = GetComponentInParent<RaceCarController>();
    }

    void UpdateEngineSFX()
    {
        float velMag = _raceCarController.GetVelocityMagnitude();

        float desiredVolume = Mathf.Abs(velMag * 0.05f);

        desiredVolume = Mathf.Clamp(desiredVolume, 0.2f, 1.0f);

        _engineAudioSource.volume = Mathf.Lerp(_engineAudioSource.volume, desiredVolume, Time.deltaTime * 10);

        desiredEnginePitch = velMag * 0.2f;

        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);

        _engineAudioSource.pitch = Mathf.Lerp(_engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }
    void UpdateTireScreechSFX()
    {
        if (_raceCarController.isScreeching(out float latVel, out bool Braking))
        {
            if (Braking)
            {
                _tireScreechingAudioSource.volume =
                    Mathf.Lerp(_tireScreechingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                _tireScreechingAudioSource.volume = Mathf.Abs(latVel) * 0.05f;
                tireScreechPitch = Mathf.Abs(latVel) * 0.1f;
            }
        }
        else
        {
            _tireScreechingAudioSource.volume = Mathf.Lerp(_tireScreechingAudioSource.volume, 0, Time.deltaTime * 10);
        }
    }
    void UpdateCarHitSFX(float relVelocity)
    {
        float volume = relVelocity * 0.1f;
        _carHitAudioSource.volume = volume;
        _carHitAudioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTireScreechSFX();
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        float relVelocity = other.relativeVelocity.magnitude;
        if (!_carHitAudioSource.isPlaying)
        {
           UpdateCarHitSFX(relVelocity); 
        }
        
    }
}
