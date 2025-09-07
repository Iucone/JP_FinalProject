
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class RainEvent : WeatherEvent
{
    [Tooltip("Rain particle system with a fog-like particle system as a child")]
    [SerializeField]
    private ParticleSystem rain;
    [SerializeField]
    private ParticleSystem fog; 
    [SerializeField]
    private float minEmissionrate = 200;
    [SerializeField]
    private float maxEmissionRate = 1500;


//    private AudioSource audioSource;
    private EmissionModule rainEmission;
    private EmissionModule fogEmission;
    private MainModule fogMain;


    void Awake()
    {
        OnAwake();
        rainEmission = rain.emission;
        fogEmission = fog.emission;
        fogMain = fog.main;
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
        //if (isStopping)
        //  return;
        IntensityUpdate(WeatherController.instance.GetEventIntensity());
        //rain.gameObject.SetActive(true);
        fogMain.simulationSpeed = 1f;
        rain.Play();
        fog.Play();
        SetIntensity(1.0f); 

        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }
     


    public override void StopEvent()
    {
        //if (isStopping)
          //  return;

        //isStopping = true;

        rain.Stop();
        fog.Stop();
        fogMain.simulationSpeed = 2f; 

        ModifyBackgroundAudioVolume(0.5f, false, true);         
    }



    protected override void IntensityUpdate(float intensity)
    {
        UpdateEmissionRate(intensity);
    }

    public override bool IsEventActive()
    {
        return rain.gameObject.activeSelf;
    }

    private void UpdateEmissionRate(float intensity)
    {
        rainEmission.rateOverTime = minEmissionrate + intensity * (maxEmissionRate - minEmissionrate);
    }

    public override bool AllowIntensityChange() => true;
}

