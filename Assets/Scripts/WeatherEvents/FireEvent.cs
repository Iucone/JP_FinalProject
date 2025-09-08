
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FireEvent : WeatherEvent
{
    [System.Serializable]
    public class LightData
    {
        public Light light;        
        public float maxIntensity;
    }

    [System.Serializable]
    public class FireData
    {
        public ParticleSystem fire;

        [HideInInspector]
        public float minRateOverTime;
        [HideInInspector]
        public float maxRateOverTime;
    }


    [Tooltip("Fire event-")]
    [SerializeField]
    //public ParticleSystem[] fires;
    private FireData[] fires;
    [SerializeField]
    private LightData[] lights;
     


    void Awake()
    {
        OnAwake();
        foreach (LightData ld in lights)
        {         
            ld.light.intensity = 0f;
        }

        foreach (FireData fd in fires)
        {
            EmissionModule emission = fd.fire.emission;
            fd.minRateOverTime = emission.rateOverTime.constant * 10f / 100f;
            fd.maxRateOverTime = emission.rateOverTime.constant;
        }
    }

    private void Update()
    { 
    }


    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }



    private IEnumerator StartLightsCO(float time)
    {
        int done = 0;
        
        while (done != lights.Length)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].light.intensity += Time.deltaTime * 400f;
                if (lights[i].light.intensity >= lights[i].maxIntensity)
                {
                    lights[i].light.intensity = lights[i].maxIntensity;
                    done++;
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    private IEnumerator StopLightsCO(float time)
    {
        int done = 0;
        while (done != lights.Length)
        {
            for (int i = 0; i < lights.Length; i++)
            { 
                lights[i].light.intensity -= Time.deltaTime * 400f;
                if (lights[i].light.intensity <= 0f)
                {
                    lights[i].light.intensity = 0f;
                    done++;
                }
            }
            yield return new WaitForSeconds( Time.deltaTime );
        }
    }

    private void StartLights()
    { 
        StopAllCoroutines();
        StartCoroutine(nameof(StartLightsCO), 2f);
    }

    private void StopLights()
    { 
        StopAllCoroutines();
        StartCoroutine(nameof(StopLightsCO), 2f);
    }


    protected override void StartEventInternal()
    {
        UpdateEmissionRate(WeatherController.instance.GetEventIntensity());
        foreach (var f in fires)
        {
            f.fire.Play();
        }
         
        StartLights();

        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    protected override void StopEventInternal()
    {
        foreach (var f in fires)
        {
            f.fire.Stop();
        }

        StopLights();

        ModifyBackgroundAudioVolume(0.5f, false, true);
    }


    protected override void IntensityUpdate(float intensity)
    {
        UpdateEmissionRate(intensity);
    }
     
    private void UpdateEmissionRate(float intensity)
    {
        foreach(FireData fd in fires)
        {
            EmissionModule emission = fd.fire.emission;
            emission.rateOverTime = fd.minRateOverTime + intensity * (fd.maxRateOverTime - fd.minRateOverTime);
        }
    }

    public override bool AllowIntensityChange() => true;
}

