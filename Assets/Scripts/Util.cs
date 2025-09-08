using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Util
{

    public static Vector3 ComputeNewTargetPos(Vector3 curPosition, Vector2 sceneCenter, float radius)
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


     

    /**
     * Crea un uovo path: precondizione è che o non c'è un vecchio path oppure se c'era
     * allora abbiamo raggiunto l'ultimo punto di tale path (in questo modo non ci saranno 
     * discontinuità di movimento.
     * 
     * Quindi startingPosition == tranform.position == path last point
     * 
     */
    public static void CreateRandomPath(CubicHermiteSpline path, Vector3 startingPosition, int numOfPoints, Vector2 sceneCenter, float radius, out float t)
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
            newPoints[i] = startingPosition = ComputeNewTargetPos(startingPosition, sceneCenter, radius);
        }
        path.Init(newPoints);

        startingIndex--;
        t = (float)(startingIndex) / (newPoints.Length - 1);
    }
     
}
