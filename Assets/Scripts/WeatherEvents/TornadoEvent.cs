using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class TornadoEvent : WeatherEvent
{
    public ParticleSystem tornado;
    public ParticleSystem[] winds;
    public Vector2 sceneCenter = new Vector2(32f, 26f);
    public float radius = 12f;
    public float speed = 0.15f;

    private float curSpeed;
    private Vector3 startingPosition;
    private int numOfPathPoints = 20;
    private Vector3 targetPos;
    private CubicHermiteSpline path = new CubicHermiteSpline();
    private float positionAlongPath = 0f;


    /*public TornadoEvent(EventName name, string description, List<WeatherParameter> conditions) : base(name, description, conditions)
    {
    }*/

    void Start()
    {
        startingPosition = tornado.transform.position;
    }

    private void Update()
    {
        if (path.GetPoints() == null)
            return;

        tornado.transform.position = path.GetPointOnClosedPath(positionAlongPath);
        positionAlongPath += curSpeed * Time.deltaTime;
         
        if (positionAlongPath >= 1.0f)
        { 
            {
                print("new tornado path");
                //positionAlongPath = 0.0f;
                CreateRandomPath(path, transform.position, numOfPathPoints);
                UpdateSpeed();
            }
        }
    }


    public override bool CanActivateEvent(WeatherState weather)
    {
        return false;
    }

     

    public override void StartEvent()
    {
        //if (tornado.isPlaying)
          //  return;

        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);


        tornado.transform.position = startingPosition;
        CreateRandomPath(path, startingPosition, numOfPathPoints);
        UpdateSpeed();


        UpdateEmissionRate();
        tornado.Play();
        foreach (var wind in winds) wind.Play();
        WeatherController.instance.SetWindIntensity(1f);
    }

    public override void StopEvent()
    {
        ModifyBackgroundAudioVolume(0.5f, false, true);

        tornado.Stop();
        foreach (var wind in winds) wind.Stop();
        WeatherController.instance.ResetWindIntensity();
        path.Reset();
    }


    protected override void IntensityUpdate()
    {
        //UpdateEmissonRate();
    }

    public override bool IsEventActive()
    {
        return false;
    }

    private void UpdateEmissionRate()
    { 
    }





    private Vector3 ComputeNewTargetPos(Vector3 curPosition)
    {
        float len = Random.Range(6f, 6f);
        Vector3 dir = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f));

        if (dir.magnitude < 0.001f)
            print("porco dio");

        Vector3 pos = curPosition +
            dir.normalized * len;


        //pos.x = 2f*(sceneCenter.x - radius) - pos.x;
        pos.y = Random.Range(2f, 3.5f);
        pos.x = Mathf.Clamp(pos.x, sceneCenter.x - radius, sceneCenter.x + radius);
        pos.z = Mathf.Clamp(pos.z, sceneCenter.y - radius, sceneCenter.y + radius);

        if ((pos - curPosition).magnitude < 0.0000001f)
            pos = curPosition + new Vector3(0.01f, 0.01f, 0.01f);

        return pos;
    }
     

    /**
     * Crea un uovo path: precondizione è che o non c'è un vecchio path oppure se c'era
     * allora abbiamo raggiunto l'ultimo punto di tale path (in questo modo non ci saranno 
     * discontinuità di movimento.
     * 
     * Quindi startingPosition == tranform.position == path last point
     * 
     */
    private void CreateRandomPath(CubicHermiteSpline path, Vector3 startingPosition, int numOfPoints)
    {
        Vector3[] oldPoints = path.GetPoints();
        Vector3[] newPoints;
        int startingIndex = 0;
        if (oldPoints != null)
        {
            if ((startingPosition - transform.position).magnitude > 0.01f)
                print("e no");
            newPoints = new Vector3[numOfPoints + 2];
            newPoints[0] = oldPoints[oldPoints.Length - 2];
            newPoints[1] = oldPoints[oldPoints.Length - 1];
            startingIndex = 2;
        }
        else
        {
            startingIndex = 1;
            newPoints = new Vector3[numOfPoints];
            newPoints[0] = startingPosition;
        }

        for (int i = startingIndex; i < newPoints.Length; i++)
        {
            newPoints[i] = startingPosition = ComputeNewTargetPos(startingPosition);
        }
        path.Init(newPoints);

        startingIndex--;
        positionAlongPath = (float)(startingIndex) / (newPoints.Length - 1);
    }

    private void UpdateSpeed()
    {
        curSpeed = 1.0f / (float)path.GetPointCount() * speed;
    }
}

