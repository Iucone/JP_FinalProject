using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DynamicClouds : WeatherEvent
{
    [Tooltip("Dynamic clouds event")]
    [SerializeField]
    public ParticleSystem clouds;
    public ParticleSystem debris;
    public ParticleSystem wind;
    public Light envLight;


    float minAlpha = 4f / 255f;
    float maxAlpha = 20f / 255f;
    float minEmissionRate = 10;
    float maxEmissionRate = 100;
    private EmissionModule fogEmission;
    bool stopped = false;
    float envLightIntensity;

    private void Start()
    {
        //fogEmission = fog.emission;
        //UpdateEmissionRate();
        envLightIntensity = envLight.intensity;
    }

    private void Update()
    {
    }


    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }



    IEnumerator ResetEnvLightIntensity()
    {
        float t = 0;
        float intensity = envLight.intensity;
        while (t <= 0.5f)
        {
            envLight.intensity = intensity + (t / 0.5f) * (envLightIntensity - intensity);
            yield return new WaitForSeconds(Time.deltaTime);
            t += Time.deltaTime;
        }
        envLight.intensity = envLightIntensity;
    }



    IEnumerator SetEnvLightIntensity(float targetIntensity)
    {
        float t = 0;
        float intensity = envLight.intensity;
        while (t <= 0.5f)
        {
            envLight.intensity = intensity + (t / 0.5f) * (targetIntensity - intensity);
            yield return new WaitForSeconds(Time.deltaTime);
            t += Time.deltaTime;
        }
        envLight.intensity = targetIntensity;
    }
    public IEnumerator SmoothFloat(Func<float> getter, Action<float> setter, float target, float duration)
    {
        float start = getter();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float value = Mathf.Lerp(start, target, t);
            setter(value);
            yield return null;
        }

        setter(target); // Assicura che il valore finale sia preciso
    }



    public override void StartEvent()
    {
        stopped = false;
        clouds.Play();
        debris.Play();
        //var main = fog.main;
        //main.simulationSpeed = 2.5f;
        //Invoke(nameof(SlowDownSimulation), 3f);

        //envLight.shadowStrength = 0.3f;


        //SetTargetValueSmoothly(ref envLight.shadowStrength, 0.3f, 0.5f);
        StartCoroutine(SmoothFloat(() => envLight.intensity, (value) => envLight.intensity = value, 2.0f, 0.5f));
        StartCoroutine(SmoothFloat(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.3f, 0.5f));
        //StartCoroutine(SetEnvLightIntensity(2.2f));

        StartBackgroundAudio();
        //IntensityUpdate();
        WeatherController.instance.SetWindIntensity(0.5f);
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    public override void StopEvent()
    {
        stopped = true;
        clouds.Stop();
        debris.Stop();
        //var main = fog.main;
        //main.simulationSpeed = 3.5f;


        //envLight.shadowStrength = 0f;
        StartCoroutine(SmoothFloat(() => envLight.intensity, (value) => envLight.intensity = value, 0.5f, 0.5f));
        StartCoroutine(SmoothFloat(() => envLight.shadowStrength, (value) => envLight.shadowStrength = value, 0.0f, 0.5f));
        //StartCoroutine(ResetEnvLightIntensity());


        ModifyBackgroundAudioVolume(0.5f, false, true);
        WeatherController.instance.ResetWindIntensity();
    }


    protected override void IntensityUpdate()
    {
        UpdateEmissionRate();

        //float inte = GetIntensity();
        //var main = fog.main;
        //main.simulationSpeed = 3f;
        //Invoke(nameof(SlowDownSimulation), 1.5f);
        //Color col = main.startColor.color;
        //main.startColor = new Color(col.r, col.g, col.b, minAlpha + GetIntensity() * (maxAlpha - minAlpha));
    }

    public override bool IsEventActive()
    {
        //return rain.gameObject.activeSelf;
        //return fog.isPlaying;
        return clouds.isPlaying;
    }

    private void UpdateEmissionRate()
    {
        fogEmission.rateOverTime = minEmissionRate + intensity * (maxEmissionRate - minEmissionRate);
    }
}

