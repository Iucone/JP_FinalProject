using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherSceneUI : MonoBehaviour
{
    [SerializeField]    
    TMP_Text eventNameUI;
    [SerializeField]
    TMP_Text eventDescUI;
    [SerializeField]
    Slider slider;



    public static WeatherSceneUI instance { get; private set; }


    private void Awake()
    {

        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    /// Chiamata quando si inizializza la prima volta e quando si cambia evento. 
    /// </summary>
    void UpdateSliderStatus()
    {
        slider.value = 1f;
        if (WeatherController.instance.AllowEventIntensityChange())
        {
            slider.enabled = true;            
        }
        else
            slider.enabled = false;

    }

    public void OnWeatherControllerInitialized()
    {
        eventNameUI.text = WeatherController.instance.GetCurrentEventName();
        eventDescUI.text = WeatherController.instance.GetCurrentEventDescription();

        UpdateSliderStatus();
    }


    /// <summary>
    /// Buttons
    /// 
    /// Unfortunately, OnClicks cannot have Enum parameters
    /// 
    /// </summary>
    /// 

    void SetEvent(WeatherEvent.EventName eventName)
    {
        WeatherController.instance.StartEvent(eventName);
        eventNameUI.text = WeatherController.instance.GetCurrentEventName();
        eventDescUI.text = WeatherController.instance.GetCurrentEventDescription();
        UpdateSliderStatus();
    }
     

    public void OnCalmWeatherButton()
    {
        SetEvent(WeatherEvent.EventName.Clear);
    }

    public void OnRainButton()
    {
        SetEvent(WeatherEvent.EventName.Rain);
    }
    public void OnStormButton()
    {
        SetEvent(WeatherEvent.EventName.Storm);
    }
    public void OnFogButton()
    {
        SetEvent(WeatherEvent.EventName.Fog);
    }
    public void OnSnowButton()
    {
        SetEvent(WeatherEvent.EventName.Snow);
    }

    public void OnAvalancheButton()
    {
        SetEvent(WeatherEvent.EventName.Avalanche);
    }

    public void OnWindButton()
    {
        SetEvent(WeatherEvent.EventName.Wind);
    }

    public void OnWildfireButton()
    {
        SetEvent(WeatherEvent.EventName.Fire);
    }


    public void OnDynamicCloudsButton()
    {
        SetEvent(WeatherEvent.EventName.DynamicClouds);
    }

    public void OnTornadoButton()
    {
        SetEvent(WeatherEvent.EventName.Tornado);
    }


    public void OnEventIntensitySlider(float value)
    {
        WeatherController.instance.SetIntensity(value);
    }
}
