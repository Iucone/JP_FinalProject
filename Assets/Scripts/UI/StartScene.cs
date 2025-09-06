using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public Button startButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(StartSimulator);
        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartSimulator();
    }


    void StartSimulator()
    {
        SceneManager.LoadScene("WeatherEffects");
    }


    IEnumerator BlinkText()
    {
        TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
        string originalText = buttonText.text;

        while (true)
        {
            buttonText.text = originalText;
            yield return new WaitForSeconds(0.5f);
            buttonText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }


}
