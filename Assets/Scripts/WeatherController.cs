using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField]
    public WeatherEvent[]   weatherEvents;
    [SerializeField]
    public WindZone windZone;
    public float defaultWindIntensity = 0.3f;



    private int activeWeaterEventIndex = -1;
    private float eventIntensity = 1f;


    public static WeatherController instance { get ; private set; }


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
        StartWeatherEvent(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            eventIntensity -= 0.1f;
            eventIntensity = Mathf.Clamp01(eventIntensity);
            UpdateCurrentWeatherEvent();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            eventIntensity += 0.1f;
            eventIntensity = Mathf.Clamp01(eventIntensity);
            UpdateCurrentWeatherEvent();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartWeatherEvent(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartWeatherEvent(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartWeatherEvent(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartWeatherEvent(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartWeatherEvent(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartWeatherEvent(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartWeatherEvent(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartWeatherEvent(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartWeatherEvent(9);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartWeatherEvent(10);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
            StartWeatherEvent(0);
        //StopCurrentWeatherEvent();
    }


    private void StartWeatherEvent(int index)
    {
        if (index == activeWeaterEventIndex)
            return;

        StopCurrentWeatherEvent();
        weatherEvents[index].StartEvent();
        activeWeaterEventIndex = index;
    }

    private void StopCurrentWeatherEvent()
    {
        if (activeWeaterEventIndex != -1)
            weatherEvents[activeWeaterEventIndex].StopEvent();
        activeWeaterEventIndex = -1;
    }


    private void UpdateCurrentWeatherEvent()
    {
        if (activeWeaterEventIndex != -1)
            weatherEvents[activeWeaterEventIndex].SetIntensity(eventIntensity);
    }


    public void SetWindIntensity(float intensity)
    {
        intensity = Mathf.Clamp01(intensity);
        windZone.windMain = intensity;
    }

    public float GetWindIntensity() => windZone.windMain;

    public void ResetWindIntensity()
    {
        windZone.windMain = defaultWindIntensity;
    }


    public float GetEventIntensity() => eventIntensity;

}
