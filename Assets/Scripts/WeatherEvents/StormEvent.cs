
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class StormEvent : WeatherEvent
{

    [Tooltip("Storm event.")]
    [SerializeField]
    public ParticleSystem rain, wind, fog;
    public GameObject lightningStrike;
    public Light envLight;

    private EmissionModule rainEmission;
    private EmissionModule windEmission;
    private float envLightIntensity;
    //private AudioSource audioSource;


    /*public FireEvent(EventName name, string description, List<WeatherParameter> conditions) : base(name, description, conditions)
    {
    }*/

    void Start()
    { 
        //lightningStrike.SetActive(false);
        envLightIntensity = envLight.intensity;
        windEmission = wind.emission;
        rainEmission = rain.emission;
        //audioSource = GetComponents<AudioSource>();
        //audioSource = GetComponent<AudioSource>();
        //audioSource.volume = 0f;
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
        //if (!audioSource.isPlaying)
        //  audioSource.Play();
        //StartCoroutine(ModifiyAudioVolume(audioSource, 0.5f, true));
    }

    public override void StopEvent()
    {
        var main = wind.main;
        main.simulationSpeed = 3f;
        rain.Stop();
        wind.Stop();
        WeatherController.instance.ResetWindIntensity();
        //lightningStrike.SetActive(false);
        StartCoroutine(ResetEnvLight());
        lightningStrike.GetComponent<Lightning>().StopEvent();
        lightningStrike.GetComponent<LightFlashes>().StopEvent();
        //StartCoroutine(ModifiyAudioVolume(audioSource, 0.5f, false, () => audioSource.Stop()));
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }

    IEnumerator ResetEnvLight()
    {
        float t = 0;
        float intensity = envLight.intensity;
        while (t <= 0.5f)
        {
            envLight.intensity = intensity + (t / 0.5f) * (envLightIntensity - intensity);
            yield return new WaitForSeconds(Time.deltaTime);
            t += Time.deltaTime;
        }
        envLight.intensity = envLightIntensity;
    }


    protected override void IntensityUpdate()
    {
        UpdateEmissionRate();
    }

    public override bool IsEventActive()
    {
        return false;
    }

    private void UpdateEmissionRate()
    {

        //EmissionModule emission = rain.emission;
        //emission.rateOverTime

        /*foreach (FireData fd in fires)
        {
            EmissionModule emission = fd.fire.emission;
            emission.rateOverTime = fd.minRateOverTime + intensity * (fd.maxRateOverTime - fd.minRateOverTime);
        }*/
    }
}

