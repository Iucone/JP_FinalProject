
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
    public ParticleSystem rain;
    [SerializeField]
    public ParticleSystem fog;

    public float minEmissionrate = 200;
    public float maxEmissionRate = 1500;


    
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

        UpdateEmissionRate();
    }

    private void Update()
    {
        /*if (!IsEventActive())
        {
            return;
        }*/
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!IsEventActive())
                StartEvent();
            else
                StopEvent();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            intensity += 0.1f;
            SetIntensity(intensity);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            intensity -= 0.1f;
            SetIntensity(intensity);
        }
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
    }

    public override void StopEvent()
    {
        //if (isStopping)
          //  return;

        //isStopping = true;

        rain.Stop();
        fog.Stop();
        fogMain.simulationSpeed = 2f;

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

    private void FinalizeStop()
    {
        rain.gameObject.SetActive(false);
        //isStopping = false;
        fogMain.simulationSpeed = 1f;
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

