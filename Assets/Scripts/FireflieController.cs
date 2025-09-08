using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class FireflieController : MonoBehaviour
{
    [SerializeField]
    private Vector2 sceneCenter = new Vector2(25f, 25f);
    [SerializeField]
    private float radius = 20f;
    [SerializeField]
    private float speed = 0.35f;

    private int numOfPathPoints = 8;//4;
    private Vector3 targetPos;
    //private Spline path = new Spline();
    private CubicHermiteSpline path = new CubicHermiteSpline();
    private float positionAlongPath = 0f;
    private bool hideMode = false;
    private float curSpeed;
    private Vector3 hidePos;
    private float hideTime = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // partiamo nascosti...
        transform.position = new Vector3(
            Random.Range(sceneCenter.x - radius, sceneCenter.x + radius),
            -3f,//Random.Range(2f, 3.5f),
            Random.Range(sceneCenter.y - radius, sceneCenter.y + radius));

        //targetPos = ComputeNewTargetPos();
        Util.CreateRandomPath(path, transform.position, numOfPathPoints, sceneCenter, radius, out positionAlongPath);
        UpdateSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        // segue il percorso lungo la spline        

        transform.position = path.GetPointOnClosedPath(positionAlongPath);
        positionAlongPath += curSpeed * Time.deltaTime;

        if (hideMode)
        {
            transform.position = Vector3.Lerp(transform.position, hidePos, hideTime);
            hideTime += Time.deltaTime;
            if (hideTime >= 1f * 2f)
            {
                FireflieSpawner.instance.CanDestroyFirefile(this);
                return;
            }
        }

        if (positionAlongPath >= 1.0f)
        {
            if (hideMode)
                positionAlongPath = 1f;
            else
            {
                //print("new path");
                //positionAlongPath = 0.0f;
                Util.CreateRandomPath(path, transform.position, numOfPathPoints, sceneCenter, radius, out positionAlongPath);
                UpdateSpeed();
            }
        }
        
    }
     

    /*
    private Vector3 ComputeNewTargetPos(Vector3 curPosition)
    {
        //if (!(curPosition.x >= sceneCenter.x - radius && curPosition.x <= sceneCenter.x + radius &&
          //   curPosition.z >= sceneCenter.y - radius && curPosition.z <= sceneCenter.y + radius))
            //print("curPosition wrong");

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
                            //print("OutOfBounds");
                        }
                    }


                }
            }

        }
        return pos;
    }
    */
     

    /**
     * Crea un uovo path: precondizione è che o non c'è un vecchio path oppure se c'era
     * allora abbiamo raggiunto l'ultimo punto di tale path (in questo modo non ci saranno 
     * discontinuità di movimento.
     * 
     * Quindi startingPosition == tranform.position == path last point
     * 
     */
    /*
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
        positionAlongPath = (float) (startingIndex) / (newPoints.Length-1);
    }
    */

    public void Hide()
    {
        hideMode = true;
        hidePos = Util.ComputeNewTargetPos(transform.position, sceneCenter, radius);
            //ComputeNewTargetPos(transform.position);
        hidePos.y = -6f;

        /*
                Vector3 newPos = ComputeNewTargetPos(transform.position);
                newPos.y = -5f;

                int index = path.GetSegmentBasePointIndex(positionAlongPath);
                if (index >= path.GetPointCount() - 2)
                    path.GetPoints()[path.GetPointCount() - 1].y = -6f;
                else
                    path.GetPoints()[index+2].y = -6f;
        */
        //path.AddPoint(newPos);
        //UpdateSpeed();
        //positionAlongPath *= (float)(path.GetPointCount()-1) / path.GetPointCount();
        //positionAlongPath = (float)index / (path.GetPointCount() - 1);

        /*
        Vector3[] positions = CreateRandomPath(transform.position, 2);
        positions[1].y = -3f;
        path.Init(positions);
        positionAlongPath = 0f;
        UpdateSpeed();
        */


        /*
        Vector3[] positions = CreateRandomPath(transform.position, 4);
        positions[2].y = -3f;
        path.Init(positions);
        positionAlongPath = 0f;
        UpdateSpeed();*/

        /*        int ind0, ind1;
                float t;*/
        //path.GetSegmentIndicesAt(positionAlongPath, out int ind0, out int ind1, out float t);       

    }

    public bool IsHiding() => hideMode;

    private void UpdateSpeed()
    {
        curSpeed = 1.0f / (float)path.GetPointCount() * speed;
    }
}
