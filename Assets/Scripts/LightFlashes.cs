using System.Collections.Generic;
using UnityEngine;

public class LightFlashes : MonoBehaviour
{
    public AudioClip[] rumbleAudioClips;
    public Light envLight;
    
    private float envLightIntensity;
    private float curFlashDuration;
    private float flashesEventTimeElapsed = 0f;
    private AudioSource audioSource;

    private bool isFlashing = false;
    private bool isEventActive = false;

    //***********


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ottiene o aggiunge un AudioSource al GameObject
        audioSource = GetComponent<AudioSource>();
        envLightIntensity = envLight.intensity;
    }



    // Update is called once per frame
    void Update()
    {
        if (!IsEventActive())
            return;

        if (isFlashing)
        {
            flashesEventTimeElapsed += Time.deltaTime;
            if (flashesEventTimeElapsed > curFlashDuration)
            {
                isFlashing = false;
                if (audioSource != null)
                    audioSource.PlayOneShot(rumbleAudioClips[Random.Range(0, rumbleAudioClips.Length)]);

                flashesEventTimeElapsed = 0f;
            }
            else
            {
                envLight.intensity += (Mathf.PerlinNoise1D(Time.time * 3.5f) - 0.5f) * 3.00f;
                envLight.intensity = Mathf.Clamp(envLight.intensity, envLightIntensity, 30f);
            }

        }
        //else envLight.intensity = Mathf.Lerp(envLight.intensity, envLightIntensity, 0.5f);
        else
        {
            envLight.intensity -= Time.deltaTime*2.0f;
            if (envLight.intensity < envLightIntensity)
                envLight.intensity = envLightIntensity;

        }

    }


    public bool StartFlashes(float duration)
    {
        if (isFlashing)
            return false;

        curFlashDuration = duration;
        flashesEventTimeElapsed = 0f;
        isFlashing = true;
        return true;
    }


    public bool IsFlashing() => isFlashing;


    public void StopEvent()
    {
        isEventActive = false;
    }

    public void StartEvent()
    {
        isEventActive = true;
        flashesEventTimeElapsed = 0f;
        isFlashing = false;
    }

    public bool IsEventActive() => isEventActive;

}
