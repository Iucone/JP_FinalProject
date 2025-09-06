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
    public Light envLight;

    float minEmissionRate = 10;
    float maxEmissionRate = 100;
    private EmissionModule fogEmission;
    bool stopped = false;
    float envLightDefIntensity;

    private void Start()
    {
        //UpdateEmissionRate();
        envLightDefIntensity = envLight.intensity;
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
        stopped = false;
        clouds.Play();
        debris.Play();

        SetFloatParameterSmoothly(() => envLight.intensity, (value) => envLight.intensity = value, 3.0f, 0.5f);
        SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.3f, 0.5f);

        StartBackgroundAudio();

        WeatherController.instance.SetWindIntensity(0.5f);
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    public override void StopEvent()
    {
        stopped = true;
        clouds.Stop();
        debris.Stop();
         
        SetFloatParameterSmoothly(() => envLight.intensity, (value) => envLight.intensity = value, envLightDefIntensity, 0.5f);
        SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.0f, 0.5f);

        ModifyBackgroundAudioVolume(0.5f, false, true);
        WeatherController.instance.ResetWindIntensity();
    }


    protected override void IntensityUpdate()
    {
        UpdateEmissionRate();
    }

    public override bool IsEventActive()
    {
        return clouds.isPlaying;
    }

    private void UpdateEmissionRate()
    {
        fogEmission.rateOverTime = minEmissionRate + intensity * (maxEmissionRate - minEmissionRate);
    }
}

