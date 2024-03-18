using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelryCtr : MonoBehaviour
{
    private Rigidbody rig;
    private RopeCtr curruntRope;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }


    private void CatchedRelease()
    {
        if (curruntRope != null)
        {
            curruntRope.Release();
        }
    }
    internal void GetCatched(RopeCtr ropeCtr)
    {
        CatchedRelease();
        curruntRope = ropeCtr;


    }

    internal void Pull(Vector3 direction, float pullForce)
    {
        rig.AddForce(direction * pullForce, ForceMode.Impulse);
        curruntRope = null;
    }
}
