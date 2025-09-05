
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class AvalancheEvent : WeatherEvent
{
    [Tooltip("Avalanche")]
    [SerializeField]
    public ParticleSystem[] avalanche;

    

    /*public RainEvent(EventName name, string description, List<WeatherParameter> conditions) : base(name, description, conditions)
    {
    }*/

    private void Start()
    {
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
        foreach (ParticleSystem p in avalanche)
            p.Play();
        WeatherController.instance.SetWindIntensity(1f);
    }

    public override void StopEvent()
    {
        foreach (ParticleSystem p in avalanche)
            p.Stop();
        WeatherController.instance.ResetWindIntensity();
    }


    protected override void IntensityUpdate()
    {
        //UpdateEmissionRate();
    }

    public override bool IsEventActive()
    {
        return avalanche[0].isPlaying;
    }

    private void UpdateEmissionRate()
    {        
    }
}

