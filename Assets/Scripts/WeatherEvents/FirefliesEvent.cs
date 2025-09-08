using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Fireflies : WeatherEvent
{
    //[Tooltip("Fireflies")]

    private Light envLight;



    void Awake()
    {
        OnAwake();        
    }

    private void Update()
    {
        if (!IsEventActive())
            return;
     
        if (FireflieSpawner.instance.GetCurrentActiveFireflies() == 0)
        {
            SpawnFireflies();
        }
    }


    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }

    private void SpawnFireflies()
    {
        int numFireflies = (int)(FireflieSpawner.instance.GetMaxFireflies() * WeatherController.instance.GetEventIntensity());
        for (int i = 0; i < numFireflies; i++)
            FireflieSpawner.instance.AddFireflie();
    }

    protected override void StartEventInternal()
    {   
        if (envLight == null)
            envLight = WeatherController.instance.GetEnvironmentLight();

        //IntensityUpdate(WeatherController.instance.GetEventIntensity());

        SpawnFireflies();


        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);

        SetFloatParameterSmoothly(() => envLight.intensity, (value) => envLight.intensity = value, 0.0f, 0.5f);
        //SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.3f, 0.5f);

    }

    protected override void StopEventInternal()
    {
        FireflieSpawner.instance.StopFireflies();

        ModifyBackgroundAudioVolume(0.5f, false, true);

        SetFloatParameterSmoothly(
            () => envLight.intensity, (value) => envLight.intensity = value,
            WeatherController.instance.GetEnvironmentLightDefaultIntensity(),
            0.5f);

        //SetFloatParameterSmoothly(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.0f, 0.5f);
    }


    protected override void IntensityUpdate(float intensity)
    {

        /*
        int targetNumFireflies = (int)(FireflieSpawner.instance.GetMaxFireflies() * WeatherController.instance.GetEventIntensity());
        int curNumFireflies = FireflieSpawner.instance.GetCurrentActiveFireflies();
        int addFireflie = math.sign(targetNumFireflies - curNumFireflies);
        targetNumFireflies - curNumFireflies
        for (int i = 0; i < math.abs(targetNumFireflies - curNumFireflies); i++)
        {
        }*/

        //UpdateEmissionRate(intensity);
        //WeatherController.instance.SetWindIntensity(0.5f + WeatherController.instance.GetEventIntensity() * 0.5f);
    }
     
    private void UpdateEmissionRate(float intensity)
    {
        //windEmission.rateOverTime = minEmissionrate + intensity * (maxEmissionRate - minEmissionrate);
    }

    public override bool AllowIntensityChange() => false;
}

