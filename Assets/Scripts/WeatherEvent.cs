using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;


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

    ParameterType type;
    private float minValue, maxValue;



    public WeatherParameter(ParameterType type, float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.type = type;
    }

    private float _parameter;
    //float parameter;

    // setter deve essere astratto e protetto (solo le sotto classi ecc...), il getter pubblico
}


public class WeatherState
{
    public WeatherParameter groundTemp = new WeatherParameter(WeatherParameter.ParameterType.GroundTemperature, -10f, 50f);

    //public abstract bool SetParameter(float value);

    public WeatherState()
    {
    }
}


public abstract class WeatherEvent : MonoBehaviour
{
    protected float intensity;

    /*
    protected ParticleSystem effectPrefab;
    protected void SetEffectPrefab(ParticleSystem effectPrefab)
    {
        this.effectPrefab = effectPrefab;
    }*/


    public abstract bool CanActivateEvent(WeatherState weather);
    public abstract string GetName();
    public abstract string GetDescription();



    public abstract void SetIntensity(float intensity);
    public float GetIntensity() => intensity;

    public abstract void StartEvent();
    public abstract void StopEvent();
}


public class RainEvent : WeatherEvent
{
    [Tooltip("Rain particle system with a fog-like particle system as a child")]
    [SerializeField]
    public ParticleSystem effect;

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

    public override string GetName() => "Rain";
    public override string GetDescription() => "When the ground temperature is not below zero and there is etc...";

    public override void SetIntensity(float intensity)
    {
        this.intensity = intensity;
    }

    public override void StartEvent()
    {
    }

    public override void StopEvent()
    {
    }
}


/*
 * Il WeatherManager si occupa solo di ricevere informazioni sullo stato del tempo e di avviare uno degli
 * effetti registrati possibili che sono attivabili (sempre il primo?).
 * 
 */
public class WeatherManager : MonoBehaviour
{
    public WeatherEvent[] weatherEvents;
    private WeatherState state = new WeatherState();

    // -1 indicates no current event active
    private int curWeatherEvent = -1;



    private void Start()
    {
        UpdateWeather();
    }

    private void Update()
    {
    }


    private void UpdateWeather()
    {
        for (int i = 0; i < weatherEvents.Length; i++)
        {
            if (weatherEvents[i].CanActivateEvent(state))
            {                
                if (i != curWeatherEvent)
                {
                    if (curWeatherEvent != -1)
                        weatherEvents[curWeatherEvent].StopEvent();

                    curWeatherEvent = i;

                    // attende 2 secondi e poi parte l'altro evento
                    weatherEvents[curWeatherEvent].StartEvent();
                }
                break;
            }
        }
    }


    public void SetWeatherParameter(WeatherParameter.ParameterType type, float value)
    {
        switch (type)
        {
            case    WeatherParameter.ParameterType.GroundTemperature:
                //state.groundTemp
                break;
        }

        UpdateWeather();
    }



    public float GetWeatherParameterValue(WeatherParameter.ParameterType type)
    {

        switch (type)
        {
            case WeatherParameter.ParameterType.GroundTemperature:
                //return state.groundTemp.value;
                break;
        }
        return 0f;
    }
}
