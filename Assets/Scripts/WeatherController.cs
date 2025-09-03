using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField]
    public WeatherEvent[]   weatherEvents;


    private int activeWeaterEventIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StopCurrentWeatherEvent();
            weatherEvents[0].StartEvent();
            activeWeaterEventIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StopCurrentWeatherEvent();
            weatherEvents[1].StartEvent();
            activeWeaterEventIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StopCurrentWeatherEvent();
            weatherEvents[2].StartEvent();
            activeWeaterEventIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StopCurrentWeatherEvent();
            weatherEvents[3].StartEvent();
            activeWeaterEventIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StopCurrentWeatherEvent();
            weatherEvents[4].StartEvent();
            activeWeaterEventIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StopCurrentWeatherEvent();
            weatherEvents[5].StartEvent();
            activeWeaterEventIndex = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
            StopCurrentWeatherEvent();
    }


    private void StopCurrentWeatherEvent()
    {
        if (activeWeaterEventIndex != -1)
            weatherEvents[activeWeaterEventIndex].StopEvent();
    }
}
