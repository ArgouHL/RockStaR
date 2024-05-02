using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrashJew : MonoBehaviour
{
    private PushableObj pushableObj;

    private void Awake()
    {
        pushableObj = GetComponentInParent<PushableObj>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Jewelry"))
        {
            Vector3 vec=transform.position - collision.transform.position;
            pushableObj.Pushed(vec.normalized * 10);
        }
    }
}
