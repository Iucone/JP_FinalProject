
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
    

    private CubicHermiteSpline  spline = new CubicHermiteSpline();

    /*public RainEvent(EventName name, string description, List<WeatherParameter> conditions) : base(name, description, conditions)
    {
    }*/

    private void Start()
    {
        rainEmission = rain.emission;
        fogEmission = fog.emission;        
        fogMain = fog.main;
        
        /*audioSource = GetComponent<AudioSource>();
        if (audioSource != null )
            audioSource.volume = 0f;*/

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
        //if (isStopping)
        //  return;

        //rain.gameObject.SetActive(true);
        fogMain.simulationSpeed = 1f;
        rain.Play();
        fog.Play();
        SetIntensity(1.0f);
        //audioSource.Play();
        /*
        if (audioSource != null)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            StartCoroutine(ModifyAudioVolume(audioSource, 0.5f, true));
        }*/
        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    void StopAudiosource()
    {
        //print("StoppingAudiosource");
        //audioSource.Stop();
    }



    public override void StopEvent()
    {
        //if (isStopping)
          //  return;

        //isStopping = true;

        rain.Stop();
        fog.Stop();
        fogMain.simulationSpeed = 2f;
        //audioSource.Stop();
        //if (audioSource != null)
        //  StartCoroutine(ModifyAudioVolume(audioSource, 0.5f, false, StopAudiosource));
        //() => audioSource.Stop());:;);
        ModifyBackgroundAudioVolume(0.5f, false, true);


        /*
        rainEmission.rateOverTime = 0f;
        ParticleSystem.MainModule main = rain.main;


        //fogEmission.rateOverTime = 0f;
        //fogMain.startColor.color;
        Color col = fogMain.startColor.color;
        fogMain.startColor = new Color(col.r, col.g, col.b, 0);
        fogMain.simulationSpeed = 2f;
        
        Invoke(nameof(FinalizeStop), 5f); 
        */
    }



    protected override void IntensityUpdate()
    {
        UpdateEmissionRate();
    }

    public override bool IsEventActive()
    {
        return rain.gameObject.activeSelf;
    }

    private void UpdateEmissionRate()
    {
        rainEmission.rateOverTime = minEmissionrate + intensity * (maxEmissionRate - minEmissionrate);
    }
}

