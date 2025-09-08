using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DefaultEvent : WeatherEvent
{ 
    private Light envLight;

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
        if (envLight == null)
            envLight = WeatherController.instance.GetEnvironmentLight();

        isEventActive = true;
        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
        
        SetFloatParameterSmoothly(() => envLight.intensity, (value) => envLight.intensity = value, 2.0f, 0.5f);
        SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.3f, 0.5f);
    }

    protected override void StopEventInternal()
    {
        isEventActive = false;
        ModifyBackgroundAudioVolume(0.5f, false, true);


        SetFloatParameterSmoothly(
            () => envLight.intensity, (value) => envLight.intensity = value,
            WeatherController.instance.GetEnvironmentLightDefaultIntensity(),
            0.5f);

        SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.0f, 0.5f);
    }


    protected override void IntensityUpdate(float intensity)
    {
    }
     

    public override bool AllowIntensityChange() => false;
}

