using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelrySystem : MonoBehaviour
{
    private JewelryCtr jewelryCtr;
    public static JewelrySystem instance;
    [SerializeField] private Transform spwanPos;
    [SerializeField] internal bool modeA;
    [SerializeField] internal bool modeB;
    internal bool instFinish=false;

    private void Awake()
    {
        instance = this;
        jewelryCtr = GetComponentInChildren<JewelryCtr>();
        instFinish = true;

    }

    //private void Start()
    //{
    //    ReSpawnJewelry();
    //}
    internal void ReSpawnJewelry()
    {
        HolesSys.instance.Shield();
        if (!modeB)
            return;
        jewelryCtr.Stop();
        jewelryCtr.Reset(spwanPos.position);

    }


}
