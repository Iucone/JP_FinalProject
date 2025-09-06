
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.ParticleSystem;

public class AvalancheEvent : WeatherEvent
{
    [Tooltip("Avalanche")]
    [SerializeField]
    public ParticleSystem[] avalanche;

    //private AudioSource audioSource;

    /*public RainEvent(EventName name, string description, List<WeatherParameter> conditions) : base(name, description, conditions)
    {
    }*/

    private void Start()
    {
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
        foreach (ParticleSystem p in avalanche)
            p.Play();
        WeatherController.instance.SetWindIntensity(1f);

        //if (!audioSource.isPlaying)
        //  audioSource.Play();
        //StartCoroutine(ModifyAudioVolume(audioSource, 0.5f, true));
        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    public override void StopEvent()
    {
        foreach (ParticleSystem p in avalanche)
            p.Stop();
        WeatherController.instance.ResetWindIntensity();

        //StartCoroutine(ModifyAudioVolume(audioSource, 0.5f, false, () => audioSource.Stop()));
        ModifyBackgroundAudioVolume(0.5f, false, true);
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

