
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class FireEvent : WeatherEvent
{
    [System.Serializable]
    public class LightData
    {
        public Light light;
        public float maxIntensity;
        [HideInInspector]
        public float intensity;
    }



    [Tooltip("Fire event-")]
    [SerializeField]
    public ParticleSystem[] fires;
    [SerializeField]
    public LightData[] lights;
     



    private EmissionModule rainEmission;
    private EmissionModule fogEmission;
    private MainModule fogMain;
    //private bool isStopping = false;
    private ParticleSystem.Particle[] particles;


    public FireEvent(EventName name, string description, List<WeatherParameter> conditions) : base(name, description, conditions)
    {
    }

    private void Start()
    {
        for (int i = 0; i < lights.Length; i++)
            lights[i].light.intensity = 0f;
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
                //float t = 1f - Mathf.Clamp01(timeElapsed / time);
                //lights[i].light.intensity = lights[i].maxIntensity * (1.0f - timeElapsed / time);
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
                //float t = 1f - Mathf.Clamp01(timeElapsed / time);
                //lights[i].light.intensity = lights[i].maxIntensity * (1.0f - timeElapsed / time);
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

        /*
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].light.intensity > 0f)
            {
                return;
            }
        }
        */
        StopAllCoroutines();
        StartCoroutine(nameof(StartLightsCO), 2f);
    }

    private void StopLights()
    {
        /*
        for (int i = 0; i < lights.Length; i++ )
        {
            if (lights[i].light.intensity > 0f)
            {
                StopAllCoroutines();
                StartCoroutine(nameof(StopLightsCO), 2f);
                break;
            }
        }*/

        StopAllCoroutines();
        StartCoroutine(nameof(StopLightsCO), 2f);
    }


    public override void StartEvent()
    {
        foreach (var f in fires)  f.Play();
         
        StartLights();
    }

    public override void StopEvent()
    {
        for (int i = 0; i < fires.Length; i++)
        {
            fires[i].Stop();
        }
         
        StopLights();
    }

    private void FinalizeStop()
    { 
    }


    protected override void IntensityUpdate()
    {
        UpdateEmissionRate();
    }

    public override bool IsEventActive()
    {
        return false;
    }

    private void UpdateEmissionRate()
    {
        //rainEmission.rateOverTime = minEmissionrate + intensity * (maxEmissionRate - minEmissionrate);
    }
}

