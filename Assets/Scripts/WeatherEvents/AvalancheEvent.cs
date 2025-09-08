
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.ParticleSystem;

public class AvalancheEvent : WeatherEvent
{
    [Tooltip("Avalanche")]
    [SerializeField]
    public ParticleSystem[] avalanche;


    private void Awake()
    {
        OnAwake();
    }

    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }


    protected override void StartEventInternal()
    {
        foreach (ParticleSystem p in avalanche)
            p.Play();
        WeatherController.instance.SetWindIntensity(1f);

        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    protected override void StopEventInternal()
    {
        foreach (ParticleSystem p in avalanche)
            p.Stop();
        WeatherController.instance.ResetWindIntensity();
        
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }


    protected override void IntensityUpdate(float intensity)
    {
    }


    public override bool AllowIntensityChange() => false;
}

