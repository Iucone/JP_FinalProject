using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WindEvent : WeatherEvent
{
    [Tooltip("Wind particle system with a debris particle system as a child")]
    [SerializeField]
    public ParticleSystem wind;
    [SerializeField]
    public ParticleSystem debris;

    public float minEmissionrate = 20;
    public float maxEmissionRate = 100;
    


    private EmissionModule windEmission, debrisEmission;
    private CubicHermiteSpline spline = new CubicHermiteSpline();
     


    private void Start()
    {
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


    public override void StartEvent()
    {
        IntensityUpdate(WeatherController.instance.GetEventIntensity());

        wind.Play();
        debris.Play();

        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    public override void StopEvent()
    { 
        wind.Stop();
        debris.Stop();        
        WeatherController.instance.ResetWindIntensity();       
        
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }


    protected override void IntensityUpdate(float intensity)
    {
        UpdateEmissionRate(intensity);
        WeatherController.instance.SetWindIntensity(0.5f + WeatherController.instance.GetEventIntensity() * 0.5f);
    }

    public override bool IsEventActive()
    {
        //return rain.gameObject.activeSelf;
        return wind.isPlaying;
    }

    private void UpdateEmissionRate(float intensity)
    {
        windEmission.rateOverTime = minEmissionrate + intensity * (maxEmissionRate - minEmissionrate);        
    }

    public override bool AllowIntensityChange() => true;
}

