using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectShowFace : MonoBehaviour
{


    private void Start()
    {
        Vector3 vector = transform.position - Camera.main.transform.position;
        vector.y = 0;
        transform.forward = vector;
    }
}
