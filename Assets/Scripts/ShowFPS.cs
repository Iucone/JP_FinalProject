using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    private float fps = 0f;
    private float alpha = 0.8f; // Coefficiente di smorzamento, più alto = più peso ai dati recenti
    private int frameCount;
    private float timeElapsed;
    private float fps2 = 0f;
    private float fpsTime = 0f;


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
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 25;
        gUIStyle.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 10, 600, 20), fps + " " + fps2, gUIStyle);
    }
}
