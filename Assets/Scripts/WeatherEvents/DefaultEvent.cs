using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DefaultEvent : WeatherEvent
{
    bool isEventActive = false;

    private void Start()
    {     
    }

    private void Update()
    {
    }


    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }


    public override void StartEvent()
    {
        isEventActive = true;
        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);
    }

    public override void StopEvent()
    {
        isEventActive = false;
        ModifyBackgroundAudioVolume(0.5f, false, true);
    }


    protected override void IntensityUpdate()
    {
    }

    public override bool IsEventActive()
    {
        return isEventActive;
    }

    private void UpdateEmissionRate()
    {
    }
}

