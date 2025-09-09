using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    GameObject infoPanel, closedInfoPanel;
    [SerializeField]
    GameObject weatherEventButtonsParent;


    public static WeatherSceneUI instance { get; private set; }


    //private List<Button> eventButtons = new List<Button>();
    private Button[]        eventButtons = null;



    private float fps = 0f;
    private float alpha = 0.8f; // Coefficiente di smorzamento, più alto = più peso ai dati recenti
    private int frameCount;
    private float timeElapsed;
    private float fps2 = 0f;
    private float fpsTime = 0f;


    private void Awake()
    {

        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        eventButtons = weatherEventButtonsParent.GetComponentsInChildren<Button>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        fps = 1f / Time.deltaTime; // Inizializza con il primo valore calcolato

        frameCount = 0;
        timeElapsed = 0;
        fps2 = 0f;

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - fpsTime > 1f)
        {
            float currentFPS = 1f / Time.deltaTime;
            fps = (currentFPS * alpha) + (fps * (1f - alpha));
            //if (fpsText != null)
            //  fpsText.text = "fps: " + (int)fps;

            timeElapsed += Time.deltaTime;
            frameCount++;
            fps2 = frameCount / timeElapsed;
            fps2 = (currentFPS * alpha) + (fps2 * (1f - alpha));

            fpsTime = Time.time;
        }
    }

    void OnGUI()
    {
        GUIStyle gs = new GUIStyle();
        gs.fontSize = 25;
        gs.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 10, 600, 20), fps + " " + fps2, gs);
        
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


    private void DisableEventButtons()
    {
        foreach (Button button in eventButtons)
        {
            button.interactable = false;
        }
    }

    private void EnableEventButtons()
    {
        foreach (Button button in eventButtons)
        {
            button.interactable = true;
        }
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
        DisableEventButtons();
        Invoke(nameof(EnableEventButtons), 2f);
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

    public void OnNocturneButton()
    {
        SetEvent(WeatherEvent.EventName.NocturneMild);
    }

    public void OnEventIntensitySlider(float value)
    {
        WeatherController.instance.SetIntensity(value);
    }






    public void OnCloseInfoPanel()
    {
        infoPanel.SetActive(false);
        closedInfoPanel.SetActive(true);
    }

    public void OnOpenInfoPanel()
    {
        infoPanel.SetActive(true);
        closedInfoPanel.SetActive(false);
    }


    public void OnExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Esce dalla Play Mode
#elif UNITY_WEBGL
        Application.OpenURL("https://google.com"); // WebGL non può chiudere l'app
#else
        Application.Quit(); // Chiude l'applicazione
#endif

    }
}
