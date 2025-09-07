using UnityEngine;

public class WeatherSceneUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Buttons
    /// </summary>
    /// 
     
     

    public void OnCalmWeatherButton()
    {
        WeatherController.instance.StartEvent(WeatherEvent.EventName.Clear);
    }


    public void OnTornadoButton()
    { 
        WeatherController.instance.StartEvent(WeatherEvent.EventName.Tornado);
    }
}
