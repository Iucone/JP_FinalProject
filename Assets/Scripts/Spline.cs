using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Unity;



/// <summary>
/// 
/// </summary> 
public class Spline
{
    private Vector3[] points;

    public Spline() { }

    public void Init(Vector3[] positions)
    {
        if (positions.Length < 4)
        {
            Debug.LogError("Sono necessari almeno 4 punti per una spline Catmull-Rom.");
            return;
        }
        this.points = positions;
    }

    /*
    public Vector3 GetInterpolatedPoint(float t)
    { 

        if (points == null || points.Length < 2)
        {
            Debug.LogError("Il path non è inizializzato correttamente.");
            return Vector3.zero;
        }

        // Assicuro che t sia compreso tra 0 e 1
        t = Mathf.Clamp01(t);

        // Numero di segmenti nel path
        int segmentCount = points.Length;

        // Calcolo il valore di t in funzione del segmento
        float totalLength = t * segmentCount; // Valore "scalato" al numero di segmenti
        int index0 = Mathf.FloorToInt(totalLength) % segmentCount; // Punto iniziale del segmento
        int index1 = (index0 + 1) % segmentCount; // Punto finale del segmento

        // Calcolo locale del parametro t per il segmento
        float localT = totalLength - Mathf.Floor(totalLength);

        // Interpolazione lineare tra i due punti del segmento
        return Vector3.Lerp(points[index0], points[index1], localT);


    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float f0 = -0.5f * t3 + t2 - 0.5f * t;
        float f1 = 1.5f * t3 - 2.5f * t2 + 1.0f;
        float f2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
        float f3 = 0.5f * t3 - 0.5f * t2;

        return f0 * p0 + f1 * p1 + f2 * p2 + f3 * p3;
    }
    */
      

    public Vector3 GetPointOnClosedPath(float t)
    {
        if (points == null || points.Length < 4)
        {
            Debug.LogError("Sono necessari almeno 4 punti per usare l'interpolazione cubica.");
            return Vector3.zero;
        }

        // Assicuro che t sia compreso tra 0 e 1
        t = Mathf.Clamp01(t);

        // Numero di segmenti nel path
        int numSegments = points.Length;

        // Calcolo il valore di t in funzione del segmento
        float totalLength = t * numSegments; // Valore "scalato" al numero di segmenti
        int index1 = Mathf.FloorToInt(totalLength) % numSegments;
        int index0 = (index1 - 1 + numSegments) % numSegments; // Punto precedente
        int index2 = (index1 + 1) % numSegments; // Punto successivo
        int index3 = (index2 + 1) % numSegments; // Punto successivo al successivo

        // Calcolo locale del parametro t per il segmento
        float localT = totalLength - Mathf.Floor(totalLength);

        // Interpolazione cubica (Catmull-Rom)
        return CatmullRom(points[index0], points[index1], points[index2], points[index3], localT);
    }



    public void GetSegmentIndicesAt(float t, out int index0, out int index1, out float localT )
    {

        // Assicuro che t sia compreso tra 0 e 1
        t = Mathf.Clamp01(t);
        // Numero di segmenti nel path
        int numSegments = points.Length;
        // Calcolo il valore di t in funzione del segmento
        float totalLength = t * numSegments; // Valore "scalato" al numero di segmenti        
        // Calcolo locale del parametro t per il segmento
        localT = totalLength - Mathf.Floor(totalLength);
        index0 = Mathf.FloorToInt(totalLength) % numSegments;
        index1 = (index0 + 1) % numSegments; // Punto successivo        
    }



    public Vector3 GetPathDirectionAt(float t)
    {

        // Assicuro che t sia compreso tra 0 e 1
        t = Mathf.Clamp01(t);
        // Numero di segmenti nel path
        int numSegments = points.Length;
        // Calcolo il valore di t in funzione del segmento
        float totalLength = t * numSegments; // Valore "scalato" al numero di segmenti
        // Calcolo locale del parametro t per il segmento
        //localT = totalLength - Mathf.Floor(totalLength);
        int index0 = Mathf.FloorToInt(totalLength) % numSegments;
        int index1 = (index0 + 1) % numSegments; // Punto successivo        
        Vector3 dir = points[index1] - points[index0];
        dir.Normalize();
        return dir;
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float f0 = -0.5f * t3 + t2 - 0.5f * t;
        float f1 = 1.5f * t3 - 2.5f * t2 + 1.0f;
        float f2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
        float f3 = 0.5f * t3 - 0.5f * t2;

        return f0 * p0 + f1 * p1 + f2 * p2 + f3 * p3;
    }


    public int GetPointCount() { return points.Length; }


    public Vector3  GetPoint(int index) { return points[index]; }
    public Vector3[]GetPoints()
    {
        return points;
    }
}


/*
 * Spline cubic-hermite con tangenti computate come differenze finite. Interpola tutti i punti
 * dati usando un singolo parametro (t da 0 a 1). Unica precondizione: dist(p_k, p_k+1) > 0.
 * 
 */
public class CubicHermiteSpline
{
    private Vector3[] points;

    public CubicHermiteSpline() { }


    public void Init(Vector3[] positions)
    {
        if (positions.Length < 2)
        {
            Debug.LogError("Sono necessari almeno 4 punti per una spline Cubic-Hermite");
            return;
        }
        this.points = positions;
    }

    
    private Vector3 Interpolate(Vector3 m0, Vector3 p0, Vector3 m1, Vector3 p1, float t, float h)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        float h00 = 2f * t3 - 3f * t2 + 1f;
        float h10 = t3 - 2f * t2 + t;
        float h01 = -2f * t3 + 3f * t2;
        float h11 = t3 - t2;        
        return h00 * p0 + h10 * h * m0 + h01 * p1 + h11 * h * m1;
    }


    public Vector3 GetPointOnClosedPath(float t)
    {
        return GetInterpolatedPoint(t);
    }

    public Vector3 GetInterpolatedPoint(float t)
    {
        t = Mathf.Clamp(t, 0f, 0.999999f);

        int baseIndex = (int) (t * (points.Length-1));

        float totalLength = t * (points.Length-1); // Valore "scalato" al numero di segmenti
        // Calcolo locale del parametro t per il segmento
        float localT = totalLength  - Mathf.Floor(totalLength);

        Vector3 p, p0, p1, m0, m1;
        p0 = points[baseIndex];
        p1 = points[baseIndex + 1];// sempre permesso
        float h = (p1 - p0).magnitude;
        
        m1 = m0 = (p1 - p0) / (2.0f * h);
        if (baseIndex - 1 >= 0) 
        {
            p = points[baseIndex - 1];
            m0 += (p0 - p) / (2.0f * (p0 - p).magnitude);
        }

        if (baseIndex + 2 < points.Length)
        {
            p = points[baseIndex + 2];
            m1 += (p-p1) / (2.0f * (p - p1).magnitude);
        }

        return Interpolate(m0, p0, m1, p1, localT, h);
    }


    public void AddPoint(Vector3 point)
    {
        Vector3[] newPoints = new Vector3[points.Length + 1];
        points.CopyTo(newPoints, 0);
        newPoints[newPoints.Length - 1] = point;
        points = newPoints;
    }


    public Vector3 GetSegmentBasePoint(float t)
    {
        t = Mathf.Clamp(t, 0f, 0.999999f);
        int baseIndex = (int)(t * (points.Length - 1));
        return points[baseIndex];
    }

    public int GetSegmentBasePointIndex(float t)
    {
        t = Mathf.Clamp(t, 0f, 0.999999f);
        return (int)(t * (points.Length - 1));
    }

    public int GetPointCount() => points.Length;

    public Vector3[] GetPoints() => points;
    public Vector3 GetPoint(int index) => points[index];
    public void SetPoint(Vector3 point, int index)
    {
        points[index] = point;
    }
}