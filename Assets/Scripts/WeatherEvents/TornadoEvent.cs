using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;
public class TornadoEvent : WeatherEvent
{
    public ParticleSystem tornado;
    public ParticleSystem rain;
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
     
    
    void Awake()
    {
        OnAwake();
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



    protected override void StartEventInternal()
    {
        //if (tornado.isPlaying)
          //  return;

        StartBackgroundAudio();
        ModifyBackgroundAudioVolume(0.5f, true, false);


        tornado.transform.position = startingPosition;
        CreateRandomPath(path, startingPosition, numOfPathPoints);

        if (false)
        {
            GameObject marker = GameObject.Find("TestCube");
            for (int i = 0; i < path.GetPointCount(); i++) 
            {
                GameObject bo = Instantiate(marker, path.GetPoints()[i], Quaternion.identity);
                Renderer r = bo.GetComponent<Renderer>();
                Color col = r.material.color;
                col.r = ((float)i / (path.GetPointCount()-1)) * 1f;
                col.g = 0f;
                col.b = 0f;
                r.material.color = col;
            }
        }

        UpdateSpeed();


        tornado.Play();
        foreach (var wind in winds) wind.Play();
        rain.Play();
        WeatherController.instance.SetWindIntensity(1f);
    }

    protected override void StopEventInternal()
    {
        ModifyBackgroundAudioVolume(0.5f, false, true);

        tornado.Stop();
        foreach (var wind in winds) wind.Stop();
        rain.Stop();

        WeatherController.instance.ResetWindIntensity();
        path.Reset();
    }


    protected override void IntensityUpdate(float intensity)
    {
    }
     


    private Vector3 ComputeNewTargetPos(Vector3 curPosition)
    {
        if (!(curPosition.x >= sceneCenter.x - radius && curPosition.x <= sceneCenter.x + radius &&
             curPosition.z >= sceneCenter.y - radius && curPosition.z <= sceneCenter.y + radius))
            print("curPosition wrong");

        Vector3 pos = Vector3.zero;
        //while (true)
        {

            float len = Random.Range(8f, 14f);
            Vector3 dir = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f));


            pos = curPosition +
                dir.normalized * len;


            //pos.x = 2f*(sceneCenter.x - radius) - pos.x;
            pos.y = Random.Range(2f, 3.5f);
            //pos.x = Mathf.Clamp(pos.x, sceneCenter.x - radius, sceneCenter.x + radius);
            //pos.z = Mathf.Clamp(pos.z, sceneCenter.y - radius, sceneCenter.y + radius);           

            //if (pos.x >= sceneCenter.x - radius && pos.x <= sceneCenter.x + radius &&
            //  pos.z >= sceneCenter.y - radius && pos.z <= sceneCenter.y + radius)
            //break;
             

            if (!(pos.x >= sceneCenter.x - radius && pos.x <= sceneCenter.x + radius &&
                pos.z >= sceneCenter.y - radius && pos.z <= sceneCenter.y + radius))
            {
                float t = dir.z;
                dir.z = -dir.x;
                dir.x = t;
                pos = curPosition + dir.normalized * len;
                pos.y = Random.Range(2f, 3.5f);
                if (!(pos.x >= sceneCenter.x - radius && pos.x <= sceneCenter.x + radius &&
                    pos.z >= sceneCenter.y - radius && pos.z <= sceneCenter.y + radius))
                {
                    t = dir.z;
                    dir.z = -dir.x;
                    dir.x = t;
                    pos = curPosition + dir.normalized * len;
                    pos.y = Random.Range(2f, 3.5f);
                    if (!(pos.x >= sceneCenter.x - radius && pos.x <= sceneCenter.x + radius &&
                        pos.z >= sceneCenter.y - radius && pos.z <= sceneCenter.y + radius))
                    {
                        t = dir.z;
                        dir.z = -dir.x;
                        dir.x = t;
                        pos = curPosition + dir.normalized * len;
                        pos.y = Random.Range(2f, 3.5f);
                        if (!(pos.x >= sceneCenter.x - radius && pos.x <= sceneCenter.x + radius &&
                            pos.z >= sceneCenter.y - radius && pos.z <= sceneCenter.y + radius))
                        {
                            print("OutOfBounds");
                        }
                    }


                }
            }

        }
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


    public override bool AllowIntensityChange() => false;
}

