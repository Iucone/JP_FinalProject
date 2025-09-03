
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
    private EventName name;
    private string description;

    /*
    protected ParticleSystem effectPrefab;
    protected void SetEffectPrefab(ParticleSystem effectPrefab)
    {
        this.effectPrefab = effectPrefab;
    }*/

    public WeatherEvent(EventName name, string description, List<WeatherParameter> conditions)
    {
        this.name = name;
        this.description = description;
        this.weatherConditions = conditions;
        intensity = 0.5f;
    }


    public virtual bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }
    public EventName GetName() => name;
    public string GetDescription() => description;



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
}

