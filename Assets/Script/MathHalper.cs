using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHalper : MonoBehaviour
{
  public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float dotProduct = Vector2.Dot(vectorA.normalized, vectorB.normalized);

        // Calculate the magnitudes of the vectors
        float magnitudeA = vectorA.magnitude;
        float magnitudeB = vectorB.magnitude;

        // Calculate the angle in radians using the dot product formula
        float angleRad = Mathf.Acos(dotProduct / (magnitudeA * magnitudeB));

        // Convert the angle from radians to degrees
        float v=Mathf.Rad2Deg * angleRad;
        if (float.IsNaN(v))
            v = 0;
        return v;

    }
}
