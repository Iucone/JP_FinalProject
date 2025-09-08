
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class StormEvent : WeatherEvent
{

    [Tooltip("Storm event.")]
    [SerializeField]
    private ParticleSystem rain, wind, fog;
    [SerializeField]
    private GameObject lightningStrike;

    private EmissionModule rainEmission;
    private EmissionModule windEmission;
    private float envLightDefIntensity;
    private Light envLight;


    void Awake()
    { 
        OnAwake();
        //lightningStrike.SetActive(false);        
        windEmission = wind.emission;
        rainEmission = rain.emission;
    }

    void Start()
    {
        envLightDefIntensity = WeatherController.instance.GetEnvironmentLightDefaultIntensity();
    }


    void Update()
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

        rain.Play();
        wind.Play();
        var main = wind.main;
        main.simulationSpeed = 1f;


        WeatherController.instance.SetWindIntensity(0.6f + WeatherController.instance.GetEventIntensity() * 0.4f);
        //lightningStrike.SetActive(true);
        lightningStrike.GetComponent<Lightning>().StartEvent();
        lightningStrike.GetComponent<LightFlashes>().StartEvent();
        
        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    protected override void StopEventInternal()
    {
        var main = wind.main;
        main.simulationSpeed = 3f;
        rain.Stop();
        wind.Stop();
        WeatherController.instance.ResetWindIntensity();
        //lightningStrike.SetActive(false);

        //StartCoroutine(ResetEnvLight());
        SetFloatParameterSmoothly(() => envLight.intensity, (value) => envLight.intensity = value, envLightDefIntensity, 0.5f);

        lightningStrike.GetComponent<Lightning>().StopEvent();
        lightningStrike.GetComponent<LightFlashes>().StopEvent();
        //StartCoroutine(ModifiyAudioVolume(audioSource, 0.5f, false, () => audioSource.Stop()));
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }
     
    protected override void IntensityUpdate(float intensity)
    {
    }
     

    public override bool AllowIntensityChange() => false;
}

