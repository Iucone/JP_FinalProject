using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class FireflieController : MonoBehaviour
{
    public Vector2 sceneCenter = new Vector2(25f, 25f);
    public float radius = 20f;
    public float speed = 0.35f;

    private int numOfPathPoints = 4;
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
        CreateRandomPath(path, transform.position, numOfPathPoints);
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
                print("new path");
                //positionAlongPath = 0.0f;
                CreateRandomPath(path, transform.position, numOfPathPoints);
                UpdateSpeed();
            }
        }
        
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

    /*
    private Vector3[] CreateRandomPath(Vector3 startingPosition, int numOfPoints)
    {
        Vector3[] positions = new Vector3[numOfPoints];
        positions[0] = startingPosition;
        for (int i = 1; i < numOfPoints; i++)
        {
            positions[i] = startingPosition = ComputeNewTargetPos(startingPosition);
        }
        return positions;
    }


    private void CreateRandomPath(CubicHermiteSpline path, Vector3 startingPosition, int numOfPoints)
    {   
        path.Init(CreateRandomPath(startingPosition, numOfPoints));

        Vector3 p = path.GetPointOnClosedPath(0f);
        p = path.GetPointOnClosedPath(0.5f);
        p = path.GetPointOnClosedPath(0.98f);
        p = path.GetPointOnClosedPath(1.0f);
        p = p;
    }

    private void CreateRandomPath2(CubicHermiteSpline path, Vector3 startingPosition, int numOfPoints)
    {
        Vector3[] points = path.GetPoints();
        if (points == null)
        {
            CreateRandomPath(path, startingPosition, numOfPoints);
            return;
        }

        Vector3[] points2 = new Vector3[numOfPoints];
        points2[0] = points[points.Length - 2];
        startingPosition = points2[1] = points[points.Length - 1];
        for (int i = 2; i < numOfPoints; i++)
        {
            points2[i] = startingPosition = ComputeNewTargetPos(startingPosition);
        }
        

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

    public void Hide()
    {
        hideMode = true;
        hidePos = ComputeNewTargetPos(transform.position);
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
