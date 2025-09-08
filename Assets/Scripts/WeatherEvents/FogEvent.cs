using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FogEvent : WeatherEvent
{
    [Tooltip("Fog event")]
    [SerializeField]
    public ParticleSystem fog;


    float minAlpha = 4f/255f;
    float maxAlpha = 20f/255f;
    float minEmissionRate = 10;
    float maxEmissionRate = 100;
    private EmissionModule fogEmission;
    bool stopped = false;

    void Awake()
    {
        OnAwake();
        fogEmission = fog.emission;
        //UpdateEmissionRate(WeatherController.instance.GetEventIntensity());        
    }

    private void Update()
    {
    }


    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }

    void SlowDownSimulation()
    {
        if (stopped)
            return;

        var main = fog.main;
        main.simulationSpeed = 1f;
    }

    protected override void StartEventInternal()
    {

        stopped = false;
        fog.Play();
        var main = fog.main;
        main.simulationSpeed = 2.5f;
        Invoke(nameof(SlowDownSimulation), 3f);
        StartBackgroundAudio();
        IntensityUpdate(WeatherController.instance.GetEventIntensity());
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    protected override void StopEventInternal()
    {
        stopped = true;
        fog.Stop();
        var main = fog.main;
        main.simulationSpeed = 3.5f;
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }


    protected override void IntensityUpdate(float intensity)
    {
        UpdateEmissionRate(intensity);

        //float inte = GetIntensity();
        //var main = fog.main;
        //main.simulationSpeed = 3f;
        //Invoke(nameof(SlowDownSimulation), 1.5f);
        //Color col = main.startColor.color;
        //main.startColor = new Color(col.r, col.g, col.b, minAlpha + GetIntensity() * (maxAlpha - minAlpha));
    }


    private void UpdateEmissionRate(float intensity)
    {
        fogEmission.rateOverTime = minEmissionRate + intensity * (maxEmissionRate - minEmissionRate);
    }


    public override bool AllowIntensityChange() => true;
}

