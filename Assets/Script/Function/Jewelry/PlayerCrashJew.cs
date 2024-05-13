using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrashJew : MonoBehaviour
{
    private PushableObj pushableObj;
    [SerializeField] private float pushforce=10; 

    private void Awake()
    {
        pushableObj = GetComponentInParent<PushableObj>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Jewelry"))
        {
    
            Vector3 vec=transform.position - collision.transform.position;
            vec.y=0;
           // Debug.Log(vec);
            pushableObj.JewPushed(vec.normalized * pushforce);     
        }
    }
}
