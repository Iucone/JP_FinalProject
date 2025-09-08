using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WindEvent : WeatherEvent
{
    [Tooltip("Wind particle system with a debris particle system as a child")]
    [SerializeField]
    private ParticleSystem wind;
    [SerializeField]
    private ParticleSystem debris;

    [SerializeField]
    private float minEmissionrate = 20;
    [SerializeField]
    private float maxEmissionRate = 100;
    


    private EmissionModule windEmission, debrisEmission;
    private CubicHermiteSpline spline = new CubicHermiteSpline();
    private Light envLight;


    void Awake()
    {
        OnAwake();
        windEmission = wind.emission;
        debrisEmission = debris.emission;        
    }

    private void Update()
    { 
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

        wind.Play();
        debris.Play();

        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);

        SetFloatParameterSmoothly(() => envLight.intensity, (value) => envLight.intensity = value, 2.0f, 0.5f);
        SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.3f, 0.5f);

    }

    protected override void StopEventInternal()
    { 
        wind.Stop();
        debris.Stop();        
        WeatherController.instance.ResetWindIntensity();       
        
        ModifyBackgroundAudioVolume(0.5f, false, true);

        SetFloatParameterSmoothly(
            () => envLight.intensity, (value) => envLight.intensity = value,
            WeatherController.instance.GetEnvironmentLightDefaultIntensity(),
            0.5f);

        SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.0f, 0.5f);
    }


    protected override void IntensityUpdate(float intensity)
    {
        UpdateEmissionRate(intensity);
        WeatherController.instance.SetWindIntensity(0.5f + WeatherController.instance.GetEventIntensity() * 0.5f);
    }
     
    private void UpdateEmissionRate(float intensity)
    {
        windEmission.rateOverTime = minEmissionrate + intensity * (maxEmissionRate - minEmissionrate);        
    }

    public override bool AllowIntensityChange() => true;
}

