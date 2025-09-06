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

    private void Start()
    {
        fogEmission = fog.emission;
        UpdateEmissionRate();        
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

    public override void StartEvent()
    {
        stopped = false;
        fog.Play();
        var main = fog.main;
        main.simulationSpeed = 2.5f;
        Invoke(nameof(SlowDownSimulation), 3f);
        StartBackgroundAudio();
        IntensityUpdate();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    public override void StopEvent()
    {
        stopped = true;
        fog.Stop();
        var main = fog.main;
        main.simulationSpeed = 3.5f;
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }


    protected override void IntensityUpdate()
    {
        UpdateEmissionRate();

        //float inte = GetIntensity();
        //var main = fog.main;
        //main.simulationSpeed = 3f;
        //Invoke(nameof(SlowDownSimulation), 1.5f);
        //Color col = main.startColor.color;
        //main.startColor = new Color(col.r, col.g, col.b, minAlpha + GetIntensity() * (maxAlpha - minAlpha));
    }

    public override bool IsEventActive()
    {
        //return rain.gameObject.activeSelf;
        return fog.isPlaying;
    }

    private void UpdateEmissionRate()
    {
        fogEmission.rateOverTime = minEmissionRate + intensity * (maxEmissionRate - minEmissionRate);
    }
}

