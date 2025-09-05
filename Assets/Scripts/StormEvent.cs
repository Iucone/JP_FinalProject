
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


    /*public FireEvent(EventName name, string description, List<WeatherParameter> conditions) : base(name, description, conditions)
    {
    }*/

    private void Start()
    {
        //lightningStrike.SetActive(false);
        envLightIntensity = envLight.intensity;
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
        WeatherController.instance.SetWindIntensity(0.6f + WeatherController.instance.GetEventIntensity()*0.4f);
        //lightningStrike.SetActive(true);
        lightningStrike.GetComponent<Lightning>().StartEvent();
        lightningStrike.GetComponent<LightFlashes>().StartEvent();
    }

    public override void StopEvent()
    {
        rain.Stop();
        wind.Stop();
        WeatherController.instance.ResetWindIntensity();
        //lightningStrike.SetActive(false);
        StartCoroutine(ResetEnvLight());
        lightningStrike.GetComponent<Lightning>().StopEvent();
        lightningStrike.GetComponent<LightFlashes>().StopEvent();
    }

    IEnumerator ResetEnvLight()
    {
        float t = 0;
        float intensity = envLight.intensity;
        while (t <= 0.5f)
        {
            envLight.intensity = intensity + (t/0.5f) * (envLightIntensity - intensity);
            yield return new WaitForSeconds(Time.deltaTime);
            t+= Time.deltaTime;
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

