using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerGun : MonoBehaviour
{
    private PlayerCtr playerCtr;
    private Transform ownPlayer=> playerCtr.transform;
    [SerializeField] private Transform powerStartPos;
    [SerializeField] private Transform shotOutPower;
    private Coroutine powerFlayingCoro;
   // private Vector3 _powerDir;
    [SerializeField] private float pushPower =5;

    [SerializeField] private float speed = 2;


  //  private JewelryCtr jewelryCtr;

    [SerializeField] private float maxDis = 5;
  //  private float pullForce;



    private void Awake()
    {
        playerCtr = GetComponentInParent<PlayerCtr>();

    
    }




    internal void Shoot(Vector3 shootDir)
    {
        shootDir.y = 0;
        shootDir = shootDir.normalized;
        //_powerDir = shootDir;
        shotOutPower.gameObject.SetActive(true);
        if (powerFlayingCoro != null)
            return;
        shotOutPower.position = powerStartPos.position;
        powerFlayingCoro = StartCoroutine(PowerFlayingIE(shootDir, maxDis));

    }

    private IEnumerator PowerFlayingIE(Vector3 shootDir, float maxDis)
    {
       
        Debug.Log("RopeF");
        //line.positionCount = 2;
        //line.SetPositions(new Vector3[] { shotOutPower.position, powerStartPos.position });
        while (Vector3.Distance(shotOutPower.position, powerStartPos.position) < maxDis)
        {
            shotOutPower.position += shootDir * Time.deltaTime * speed;

            yield return null;
        }
        powerFlayingCoro = null;
        PowerDisapper();
    }



    private void PowerDisapper()
    {
        if (powerFlayingCoro != null)
        {
            StopCoroutine(powerFlayingCoro);
            powerFlayingCoro = null;
        }
        shotOutPower.position = powerStartPos.position;
        Debug.Log("RopeStop");      
        shotOutPower.gameObject.SetActive(false);

    }


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.gameObject.name); 
        

        if (other.TryGetComponent(out PushableObj pullableObj))
        {
            
            if (pullableObj.transform == ownPlayer)
            {
                Debug.Log("s");
                return;
            }
              
            PushObj(pullableObj);
            PowerDisapper();
        }



    }

    private void PushObj(PushableObj pullableObj)
    {
        Vector3 _powerDir = pullableObj.transform.position - shotOutPower.position;
        _powerDir.y = 0;
        _powerDir=_powerDir.normalized;
        pullableObj.Pushed(_powerDir* pushPower);

    }

   
}
