using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RopeCtr : MonoBehaviour
{
    private PlayerCtr playerCtr;

    [SerializeField] private Transform ropeHead;
    [SerializeField] private Transform ropeEnd;
    private Coroutine ropeFlayingCoro;
    private Coroutine ropePullCoro;
    [SerializeField] private LineRenderer line;
    [SerializeField] private float speed = 2;
    [SerializeField] private PullChargeUI pullChargeUI;

    private JewelryCtr jewelryCtr;

    [SerializeField] private bool catchedJew = false;

    [SerializeField] private float accelerateForce = 5;
    [SerializeField] private float maxForce = 5;
    private float pullForce;



    private void Awake()
    {
        playerCtr = GetComponentInParent<PlayerCtr>();
        line.positionCount = 0;
    }

    internal void Shoot(Vector2 shootDir, float maxDis)
    {
        ropeEnd.gameObject.SetActive(true);
        if (ropeFlayingCoro != null)
            return;
        ropeEnd.position = ropeHead.position;
        ropeFlayingCoro = StartCoroutine(ropeFlayingIE(shootDir, maxDis));

    }

    private IEnumerator ropeFlayingIE(Vector2 shootDir, float maxDis)
    {
        Debug.Log("RopeF");
        line.positionCount = 2;
        line.SetPositions(new Vector3[] { ropeEnd.position, ropeHead.position });
        while (Vector3.Distance(ropeEnd.position, ropeHead.position) < maxDis)
        {
            ropeEnd.position += new Vector3(shootDir.x, 0, shootDir.y) * Time.deltaTime * speed;
            UpdateLineRender();
            yield return null;
        }
        ropeFlayingCoro = null;
        RopeStop();
    }

    private void UpdateLineRender()
    {
        line.SetPosition(1, ropeEnd.position);
    }

    private void StopRender()
    {
        line.positionCount = 0;
    }

    private void RopeStop()
    {
        Debug.Log("RopeStop");
        playerCtr.ropeCatching = false;
        ropeEnd.gameObject.SetActive(false);
        StopRender();
    }


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.gameObject.name);
        if (ropeFlayingCoro == null)
            return;
        StopCoroutine(ropeFlayingCoro);
        ropeFlayingCoro = null;

        if (other.gameObject.CompareTag("Jewelry"))
        {
            CatchJew(other.transform);

        }

        // RopeStop();

    }

    private void CatchJew(Transform jewTransform)
    {
        ropeEnd.position = jewTransform.position;
        UpdateLineRender();
        jewelryCtr = jewTransform.GetComponentInParent<JewelryCtr>();
        jewelryCtr.GetCatched(this);
        catchedJew = true;
    }

    internal void ChargePullJew()
    {
        if (!catchedJew)
            return;
        if (ropePullCoro != null)
            return;
       
        ropePullCoro = StartCoroutine(ChargePullIE());
    }

    internal void StopChargePullJew()
    {   
        if (ropePullCoro == null)
            return;
        StopCoroutine(ropePullCoro);
        ropePullCoro = null;

        if(jewelryCtr!=null)
        PullJew();
    }

    private IEnumerator ChargePullIE()
    {
        pullForce = 0;
        do
        {
            Debug.Log("Charging");
            pullForce += accelerateForce * Time.deltaTime;
            pullChargeUI.UpdateCharge(pullForce/ maxForce);
            yield return null;
        }
        while (pullForce < maxForce);
        pullForce = maxForce;
        ropePullCoro = null;
        PullJew();
    }

    private void PullJew()
    {
        pullChargeUI.UpdateCharge(0);
        Debug.Log("PullJew");
        Vector3 direction = transform.position- jewelryCtr.transform.position;
        direction.y = 0;
        direction = direction.normalized;
        Debug.Log(direction);
        jewelryCtr.Pull(direction, pullForce);
        jewelryCtr = null;
        RopeStop();
    }

    internal void Release()
    {
        jewelryCtr = null;
        RopeStop();
        pullChargeUI.UpdateCharge(0);
        if (ropePullCoro == null)
            return;
        StopCoroutine(ropePullCoro);
        ropePullCoro = null;
    }

}
