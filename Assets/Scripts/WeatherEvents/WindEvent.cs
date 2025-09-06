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
    //private AudioSource audioSource;

    /*public RainEvent(EventName name, string description, List<WeatherParameter> conditions) : base(name, description, conditions)
    {
    }*/

    private void Start()
    {
        windEmission = wind.emission;
        debrisEmission = debris.emission;
        
        //audioSource = GetComponent<AudioSource>();
        //audioSource.volume = 0f;

        UpdateEmissionRate();
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
        wind.Play();
        debris.Play();
        //SetIntensity(1.0f);
        WeatherController.instance.SetWindIntensity(0.5f + WeatherController.instance.GetEventIntensity()*0.5f);

        //if (!audioSource.isPlaying)
        //audioSource.Play();
        //StartCoroutine(ModifyAudioVolume(audioSource, 0.5f, true));


        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    public override void StopEvent()
    {
        //if (isStopping)
        //  return;

        //isStopping = true;

        wind.Stop();
        debris.Stop();        
        WeatherController.instance.ResetWindIntensity();
        
        //StartCoroutine(ModifyAudioVolume(audioSource, 0.5f, false, ()=>audioSource.Stop()));
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }


    protected override void IntensityUpdate()
    {
        UpdateEmissionRate();
        WeatherController.instance.SetWindIntensity(0.5f + WeatherController.instance.GetEventIntensity() * 0.5f);
    }

    public override bool IsEventActive()
    {
        //return rain.gameObject.activeSelf;
        return wind.isPlaying;
    }

    private void UpdateEmissionRate()
    {
        windEmission.rateOverTime = minEmissionrate + intensity * (maxEmissionRate - minEmissionrate);        
    }
}

