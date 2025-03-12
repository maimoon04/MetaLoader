using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIndCubeCenter : MonoBehaviour
{
    public List<Transform> cubePoints;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public Transform nearestpoint(List<Transform> points)
    {
        if (cubePoints == null || cubePoints.Count == 0)
        {
            return null;
        }

        Vector3 center = Vector3.zero;
        foreach (Transform t in cubePoints) 
        {
            center += t.position;
        }
        center /= cubePoints.Count;

        Transform closest = cubePoints[0];
        float min = (closest.position - center).magnitude;

        foreach (Transform t in cubePoints)
        {
            float mag = (t.position - center).magnitude;
            if (mag < min)
            {
                min = mag;
                closest = t;
            }
        }
        return closest;
    }
}
