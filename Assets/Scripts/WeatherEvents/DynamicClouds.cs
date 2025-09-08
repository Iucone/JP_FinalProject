using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DynamicClouds : WeatherEvent
{
    [Tooltip("Dynamic clouds event")]
    [SerializeField]
    public ParticleSystem clouds;
    public ParticleSystem debris;
    public ParticleSystem wind;    
    public MinMaxParameter minMaxCloudsSpeed;

    bool stopped = false;
    Light envLight;

    void Awake()
    {
        OnAwake();        
    }
     

    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }


    protected override void StartEventInternal()
    {
        if (envLight == null)
            envLight = WeatherController.instance.GetEnvironmentLight();

        IntensityUpdate(WeatherController.instance.GetEventIntensity());

        stopped = false;

        clouds.Play();
        debris.Play();

        //IntensityUpdate(WeatherController.instance.GetEventIntensity());

        SetFloatParameterSmoothly(() => envLight.intensity, (value) => envLight.intensity = value, 3.0f, 0.5f);
        SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.3f, 0.5f);

        StartBackgroundAudio();        
        ModifyBackgroundAudioVolume(0.5f, true, false);

        WeatherController.instance.SetWindIntensity(0.8f); 
    }

    protected override void StopEventInternal()
    {
        stopped = true;

        clouds.Stop();
        debris.Stop();
         
        SetFloatParameterSmoothly(
            () => envLight.intensity, (value) => envLight.intensity = value, 
            WeatherController.instance.GetEnvironmentLightDefaultIntensity(), 
            0.5f);

        SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.0f, 0.5f);

        ModifyBackgroundAudioVolume(0.5f, false, true);
        WeatherController.instance.ResetWindIntensity();
    }


    protected override void IntensityUpdate(float intensity)
    {
        MainModule main;
        main = clouds.main;
        //main.startSpeedMultiplier = 0.5f + intensity * 0.5f;
        main.startSpeed = minMaxCloudsSpeed.minValue + intensity * (minMaxCloudsSpeed.maxValue - minMaxCloudsSpeed.minValue);
        //main = debris.main;
        //main.startSpeedMultiplier = 0.5f + intensity * 0.5f;
        //main = wind.main;
        //main.startSpeedMultiplier = 0.5f + intensity * 0.5f;

        WeatherController.instance.SetWindIntensity(0.5f + intensity * 0.3f);
    }
     

    public override bool AllowIntensityChange() => true;

}

