
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
     



    private void Start()
    {
    }

    private void Update()
    { 
    }


    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }


    public override void StartEvent()
    {
        foreach (ParticleSystem p in avalanche)
            p.Play();
        WeatherController.instance.SetWindIntensity(1f);

        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    public override void StopEvent()
    {
        foreach (ParticleSystem p in avalanche)
            p.Stop();
        WeatherController.instance.ResetWindIntensity();
        
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }


    protected override void IntensityUpdate(float intensity)
    {
    }

    public override bool IsEventActive()
    {
        return avalanche[0].isPlaying;
    }


    public override bool AllowIntensityChange() => false;
}

