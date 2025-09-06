
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;


/*
 * Impostati via codice.
 * 
 */
public class WeatherParameter
{
    public enum ParameterType
    {
        GroundTemperature,
        GroundHumidity,
        UpperAirTemperature,
        UpperAirHumidity,
        CloudHeight,
        WindSpeed,
        SnowAccumulation,
        Pressure
    }

    public enum ParameterValue
    {
        Unspecified,
        Low,
        Medium,
        High
    }

    public ParameterType type;
    public ParameterValue value;
    //private float minValue, maxValue;


    public WeatherParameter(ParameterType type)
    {
        this.type = type;
        value = ParameterValue.Unspecified;
    }


    public WeatherParameter(ParameterType type, ParameterValue value)
    {
        this.type = type;
        this.value = value;
    }


    /*public WeatherParameter(ParameterType type, float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.type = type;
    }

    private float _parameter;
    //float parameter;

    // setter deve essere astratto e protetto (solo le sotto classi ecc...), il getter pubblico*/
}


public class WeatherState
{
    //public WeatherParameter groundTemp = new WeatherParameter(WeatherParameter.ParameterType.GroundTemperature, -10f, 50f);

    //public abstract bool SetParameter(float value);

    WeatherParameter groundTemp = new WeatherParameter(WeatherParameter.ParameterType.GroundTemperature);

    public WeatherState()
    {
    }
}


public class WeatherEventCondition
{
    //List<WeatherParameter>  
}


public abstract class WeatherEvent : MonoBehaviour
{

    public enum EventName
    {
        Fog,
        Rain,
        Snow,
        Wind,
        Fire,
        Avalanche,
        Lightning
    }




    protected List<WeatherParameter> weatherConditions;
    protected float intensity;
    private EventName eventName;
    private string eventDescription;
    protected AudioSource[] backgroundAudioSources;


    protected void SetBackgroundAudioVolume(float volume)
    {
        if (backgroundAudioSources == null)
            return;

        foreach (AudioSource audioSource in backgroundAudioSources)
        {
            audioSource.volume = volume;
        }
    }
     

    private void Awake()
    {
        backgroundAudioSources = GetComponents<AudioSource>();
        SetBackgroundAudioVolume(0f);
    }


    /*
    protected ParticleSystem effectPrefab;
    protected void SetEffectPrefab(ParticleSystem effectPrefab)
    {
        this.effectPrefab = effectPrefab;
    }*/

    /*
    public WeatherEvent(EventName eventName, string eventDescription, List<WeatherParameter> conditions)
    {
        print("WeatherEvent costruttore " + eventName);
        this.eventName = eventName;
        this.eventDescription = eventDescription;
        this.weatherConditions = conditions;
        intensity = 0.5f;
    }*/


    public virtual bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }
    public EventName GetEventName() => eventName;
    public string GetEventDescription() => eventDescription;



    //public abstract void SetIntensity(float intensity);
    public void SetIntensity(float intensity)
    {
        this.intensity = Mathf.Clamp01(intensity);
        IntensityUpdate();

    }

    protected abstract void IntensityUpdate();

    public float GetIntensity() => intensity;

    public abstract void StartEvent();
    public abstract void StopEvent();
    public abstract bool IsEventActive();


    protected void StartBackgroundAudio()
    {
        if (backgroundAudioSources == null)
            return;

        for (int i = 0; i < backgroundAudioSources.Length; i++)
            backgroundAudioSources[i].Play();
    }
     
    void ModifyVolumeCallback(AudioSource audioSource, bool stopWhenMuted)
    {
        if (stopWhenMuted)
        {
            if (audioSource.volume == 0f)
                audioSource.Stop();
        }
    }

    

    protected void ModifyBackgroundAudioVolume(float duration, bool toMax, bool stopWhenMuted, float updateFreq = 20f)
    {
        if (backgroundAudioSources == null)
            return;

        Action<AudioSource, bool> callback = (audioSource, stopWhenMuted) =>
        {
            if (stopWhenMuted && audioSource.volume == 0f)
                audioSource.Stop();
        };

        for (int i = 0; i < backgroundAudioSources.Length; i++)
        {
            StartCoroutine(ModifyAudioVolume(backgroundAudioSources[i], duration, toMax, stopWhenMuted,
                callback,
                updateFreq));
        }
    }



    protected IEnumerator ModifyAudioVolume(AudioSource audioSource, float duration, bool toMax, bool stopWhenMuted,
        //Action callback = null, 
        System.Action<AudioSource, bool> callback,
        float updateFreq = 20f)
    {
        if (updateFreq <= 0f)
            updateFreq = 1f;

        float te = 0f;
        float deltaVol;
        if (toMax)
            deltaVol = (1f - audioSource.volume) / (duration * updateFreq);
        else
            deltaVol = -audioSource.volume / (duration * updateFreq);

        updateFreq = 1f / updateFreq;


        while (te < duration)
        {
            audioSource.volume += deltaVol;
            yield return new WaitForSeconds(updateFreq);
            te += updateFreq;

        }
        audioSource.volume = toMax ? 1f : 0f;
        callback?.Invoke(audioSource, stopWhenMuted);
    }

    protected IEnumerator ModifyAudioVolume(AudioSource audioSource, float duration, bool toMax, 
        Action callback = null,         
        float updateFreq = 20f)
    {
        if (updateFreq <= 0f)
            updateFreq = 1f;

        float te = 0f;
        float deltaVol;
        if (toMax)
            deltaVol = (1f - audioSource.volume) / (duration * updateFreq);
        else
            deltaVol = -audioSource.volume / (duration * updateFreq);

        updateFreq = 1f / updateFreq;


        while (te < duration)
        {
            audioSource.volume += deltaVol;
            yield return new WaitForSeconds(updateFreq);
            te += updateFreq;

        }
        audioSource.volume = toMax ? 1f : 0f;
        callback?.Invoke();
    }



    private IEnumerator SetFloatParameterSmoothlyCR(Func<float> getter, Action<float> setter, float target, float duration)
    {
        float start = getter();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float value = Mathf.Lerp(start, target, t);            
            setter(value);
            yield return null;
        }

        setter(target); // Assicura che il valore finale sia preciso
    }

    public void SetFloatParameterSmoothly(Func<float> getter, Action<float> setter, float target, float duration)
    {
        StartCoroutine(SetFloatParameterSmoothlyCR( getter, setter, target, duration));
    }
}

