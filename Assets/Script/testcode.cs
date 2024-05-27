using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcode : MonoBehaviour
{
    public Transform orgObj;
    public Vector3 orgPos;

    public int nowStep;
    bool isUpward = true;

    private void Start()
    {
        orgPos = orgObj.position;
    }

    public float stepHeight;
   
    [ContextMenu("UpOrDown")]
    public void UpOrDown()
    {
        if (nowStep == 0)
        {
            isUpward = true;
        }
        else if (nowStep == 2)
        {
            isUpward = false;
        }
        
        if (isUpward)
        {
            nowStep++;
        }
        else
        {
            nowStep--;
        }

        orgObj.position = orgPos+ new Vector3(0,stepHeight* nowStep, 0);

    }
    
}