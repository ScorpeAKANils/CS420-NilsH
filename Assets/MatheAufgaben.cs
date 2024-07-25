using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatheAufgaben : MonoBehaviour
{
    public Transform Ebene;
    public Transform Rechteck;
    public Transform a;
    public Transform b;
    public Transform c;
    public Transform d;
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
        FindClosestPoint(); 
    }

    public void AufgabeEinsA()
    {
        Vector3 ebeneNormale = NormalizeVector(Ebene.position);
        float d = VectorProjection(ebeneNormale, transform.position);
        Distance = Mathf.Abs(d) / Magnitude(ebeneNormale);
        Debug.Log(Distance);
    }

    public void AufgabeEinsB()
    {
        Vector2 rechteckMin = new Vector2(Rechteck.transform.position.x - (Rechteck.transform.localScale.x / 2), Rechteck.transform.position.y - (Rechteck.transform.localScale.y / 2));
        Vector2 rechteckMax = new Vector2(Rechteck.transform.position.x + (Rechteck.transform.localScale.x / 2), Rechteck.transform.position.y + (Rechteck.transform.localScale.y / 2));

        float dX = Mathf.Max(rechteckMin.x - transform.position.x, 0, transform.position.x - rechteckMax.x);
        float dY = Mathf.Max(rechteckMin.y- transform.position.y, 0, transform.position.y - rechteckMax.y);

        Distance = Mathf.Sqrt((dX * dX) + (dY * dY));
        Debug.Log(Distance);
       
    }


    public void Aufgabe2()
    {
        Vector3 normal1 = Ebene.up;
        Vector3 normal2 = transform.up;

        Vector3 position1 = Ebene.position;
        Vector3 position2 = transform.position;

        float a1 = normal1.x;
        float b1 = normal1.y;
        float c1 = normal1.z;
        float d1 = -(a1 * position1.x + b1 * position1.y + c1 * position1.z);

        float a2 = normal2.x;
        float b2 = normal2.y;
        float c2 = normal2.z;
        float d2 = -(a2 * position2.x + b2 * position2.y + c2 * position2.z);

        Vector3 direction = Vector3.Cross(normal1, normal2);

        if (direction != Vector3.zero)
        {
            // We choose an arbitrary value for z
            float z = 1.0f; // You can choose any non-zero value for z

            // Solve for x and y with the chosen z
            float determinant = a1 * b2 - a2 * b1;
            if (Mathf.Abs(determinant) > Mathf.Epsilon)
            {
                float x = (b1 * d2 - b2 * d1 + z * (b1 * c2 - b2 * c1)) / determinant;
                float y = (a2 * d1 - a1 * d2 + z * (a2 * c1 - a1 * c2)) / determinant;

                Vector3 pointOnLine = new Vector3(x, y, z);

                // Define two points along the direction vector for visualization
                Vector3 startPoint = pointOnLine - 10 * direction;
                Vector3 endPoint = pointOnLine + 10 * direction;

                Debug.DrawLine(startPoint, endPoint*100, Color.yellow, 1f);
            }
            else
            {
                Debug.LogError("The planes are parallel or coincident, no unique intersection line exists.");
            }
        }
        else
        {
            Debug.LogError("The planes are parallel or coincident, no unique intersection line exists.");
        }
    }

    void FindClosestPoint()
    {
        float distanceToA = Magnitude(a.position - d.position);
        float distanceToB = Magnitude(b.position - d.position);
        float distanceToC = Magnitude(c.position - d.position);

        float minDistance = Mathf.Min(distanceToA, distanceToB, distanceToC);


        if (minDistance == distanceToA)
        {
            closestPoint.GetComponent<MeshRenderer>().material.color = Color.white;
            closestPoint = a;
            closestPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else if (minDistance == distanceToB)
        {
            closestPoint.GetComponent<MeshRenderer>().material.color = Color.white;
            closestPoint = b;
            closestPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else
        {
            closestPoint.GetComponent<MeshRenderer>().material.color = Color.white;
            closestPoint = c;
            closestPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        closestPoint.GetComponent<MeshRenderer>().material.color = Color.blue; 
        Debug.Log("Der Punkt, der am nächsten zu D liegt, ist: " + closestPoint.name);
    }

    public Vector3 NormalizeVector(Vector3 vector)
    {
        return vector.normalized;
    }

    public float Magnitude(Vector3 vec)
    {
        return vec.magnitude;
    }

    public float VectorProjection(Vector3 a, Vector3 b)
    {
        return Vector3.Dot(a, b);
    }

}
