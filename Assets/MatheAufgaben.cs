using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Aufgaben
{
    A1a = 1, 
    A1b = 2, 
    A2 = 3, 
    A3 = 4

}
public class MatheAufgaben : MonoBehaviour
{
    public Transform Ebene;
    public Transform Rechteck;
    public Transform a;
    public Transform b;
    public Transform c;
    public Transform d;
    public Aufgaben aktuelleAufgabe; 
    public float Distance;
    Transform closestPoint;
    // Start is called before the first frame update
    void Start()
    {
        closestPoint = a; 
    }

    // Update is called once per frame
    void Update()
    {
        switch (aktuelleAufgabe) 
        {
            case Aufgaben.A1a:
                AufgabeEinsA();
                break;
            case Aufgaben.A1b:
                AufgabeEinsB();
                break;
            case Aufgaben.A2:
                Aufgabe2();
                break;
            case Aufgaben.A3:
                FindClosestPoint();
                break;
        }
    }

    public void AufgabeEinsA()
    {
        Vector3 ebeneNormale = NormalizeVector(Ebene.position);
        float d = VectorProjection(ebeneNormale, transform.position);
        Distance = Mathf.Abs(d) / Magnitude(ebeneNormale);
        Debug.DrawLine(transform.position, Ebene.position, Color.white); 
    }

    public void AufgabeEinsB()
    {
        Vector2 rechteckMin = new Vector2(Rechteck.transform.position.x - (Rechteck.transform.localScale.x / 2), Rechteck.transform.position.y - (Rechteck.transform.localScale.y / 2));
        Vector2 rechteckMax = new Vector2(Rechteck.transform.position.x + (Rechteck.transform.localScale.x / 2), Rechteck.transform.position.y + (Rechteck.transform.localScale.y / 2));
        float dX = Mathf.Max(rechteckMin.x - transform.position.x, 0, transform.position.x - rechteckMax.x);
        float dY = Mathf.Max(rechteckMin.y - transform.position.y, 0, transform.position.y - rechteckMax.y);
        Distance = Mathf.Sqrt((dX * dX) + (dY * dY)); 
        Debug.Log(Distance);
        Vector2 nearestPoint = new Vector2(
            Mathf.Clamp(transform.position.x, rechteckMin.x, rechteckMax.x),
            Mathf.Clamp(transform.position.y, rechteckMin.y, rechteckMax.y)
        );
        Debug.DrawLine(transform.position, nearestPoint, Color.red);
    }



    public void Aufgabe2()
    {
        // Normalenvektoren der Ebenen
        Vector3 normal1 = Ebene.up;
        Vector3 normal2 = transform.up;

        // Positionen der Ebenen
        Vector3 position1 = Ebene.position;
        Vector3 position2 = transform.position;

        // Ebenengleichungen
        float a1 = normal1.x;
        float b1 = normal1.y;
        float c1 = normal1.z;
        float d1 = -(a1 * position1.x + b1 * position1.y + c1 * position1.z);

        float a2 = normal2.x;
        float b2 = normal2.y;
        float c2 = normal2.z;
        float d2 = -(a2 * position2.x + b2 * position2.y + c2 * position2.z);

        // Richtungsvektor der Schnittgerade
        Vector3 direction = Vector3.Cross(normal1, normal2);

        if (direction != Vector3.zero)
        {
            // Finden eines Punktes auf der Schnittgeraden
            // Wir setzen z = 0 zur Berechnung
            float z = 0;
            float determinant = a1 * b2 - a2 * b1;

            if (Mathf.Abs(determinant) > Mathf.Epsilon)
            {
                float x = (b1 * d2 - b2 * d1) / determinant;
                float y = (a2 * d1 - a1 * d2) / determinant;

                Vector3 pointOnLine = new Vector3(x, y, z);
                Vector3 startPoint = pointOnLine - 10 * direction;
                Vector3 endPoint = pointOnLine + 10 * direction;

                Debug.DrawLine(startPoint, endPoint, Color.yellow);


                // Debug: Punkte auf den Ebenen
                Debug.Log($"Schnittpunkt der Geraden: {pointOnLine}");
                Debug.Log($"Ebenen Normale 1: {normal1}, Position 1: {position1}");
                Debug.Log($"Ebenen Normale 2: {normal2}, Position 2: {position2}");
            }
            else
            {
                Debug.LogError("Die Ebenen sind parallel oder übereinstimmend, es existiert keine eindeutige Schnittgerade.");
            }
        }
        else
        {
            Debug.LogError("Die Ebenen sind parallel oder übereinstimmend, es existiert keine eindeutige Schnittgerade.");
        }
    }



    void FindClosestPoint()
    {
        float distanceToA = Magnitude(a.position - d.position);
        float distanceToB = Magnitude(b.position - d.position);
        float distanceToC = Magnitude(c.position - d.position);

        float minDistance = Mathf.Min(distanceToA, distanceToB, distanceToC);
        Transform closest = distanceToA <= distanceToB && distanceToA <= distanceToC ? a : (distanceToB <= distanceToC ? b : c);

        Vector3 closestEdgePoint;
        float distanceToEdgeAB = DistanceToLineSegment(a.position, b.position, d.position, out closestEdgePoint);
        Vector3 closestEdgeAB = closestEdgePoint;
        float distanceToEdgeBC = DistanceToLineSegment(b.position, c.position, d.position, out closestEdgePoint);
        Vector3 closestEdgeBC = closestEdgePoint;
        float distanceToEdgeCA = DistanceToLineSegment(c.position, a.position, d.position, out closestEdgePoint);
        Vector3 closestEdgeCA = closestEdgePoint;

        Debug.DrawLine(a.position, b.position, Color.yellow);
        Debug.DrawLine(b.position, c.position, Color.yellow);
        Debug.DrawLine(c.position, a.position, Color.yellow);

        minDistance = Mathf.Min(minDistance, distanceToEdgeAB, distanceToEdgeBC, distanceToEdgeCA);

        if (minDistance == distanceToA)
        {
            Debug.DrawLine(d.position, a.position, Color.green);
        }
        else if (minDistance == distanceToB)
        {
            Debug.DrawLine(d.position, b.position, Color.green);
        }
        else if (minDistance == distanceToC)
        {
            Debug.DrawLine(d.position, c.position, Color.green);
        }
        else if (minDistance == distanceToEdgeAB)
        {
            Debug.DrawLine(d.position, closestEdgeAB, Color.blue);
            Debug.DrawLine(a.position, b.position, Color.red);
        }
        else if (minDistance == distanceToEdgeBC)
        {
            Debug.DrawLine(d.position, closestEdgeBC, Color.blue);
            Debug.DrawLine(b.position, c.position, Color.red);
        }
        else if (minDistance == distanceToEdgeCA)
        {
            Debug.DrawLine(d.position, closestEdgeCA, Color.blue);
            Debug.DrawLine(c.position, a.position, Color.red);
        }

        closestPoint.GetComponent<MeshRenderer>().material.color = Color.white;
        closestPoint = minDistance == distanceToA ? a : (minDistance == distanceToB ? b : (minDistance == distanceToC ? c : closestPoint));
        closestPoint.GetComponent<MeshRenderer>().material.color = Color.blue;

        Debug.Log("Der Punkt, der am nächsten zu D liegt, ist: " + closestPoint.name);
    }

    float Magnitude(Vector3 vector)
    {
        return vector.magnitude;
    }

    float DistanceToLineSegment(Vector3 v1, Vector3 v2, Vector3 p, out Vector3 closestPoint)
    {
        Vector3 segment = v2 - v1;
        Vector3 toPoint = p - v1;
        float segmentLengthSquared = segment.sqrMagnitude;

        if (segmentLengthSquared == 0f)
        {
            closestPoint = v1;
            return Magnitude(p - v1);
        }

        float t = Mathf.Clamp01(Vector3.Dot(toPoint, segment) / segmentLengthSquared);
        closestPoint = v1 + t * segment;
        return Magnitude(p - closestPoint);
    }


    public Vector3 NormalizeVector(Vector3 vector)
    {
        return vector.normalized;
    }

    public float VectorProjection(Vector3 a, Vector3 b)
    {
        return Vector3.Dot(a, b);
    }

}
