using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelrySystem : MonoBehaviour
{
    private JewelryCtr jewelryCtr;
    public static JewelrySystem instance;


    private void Awake()
    {
        instance = this;
        jewelryCtr = GetComponentInChildren<JewelryCtr>();
        
    }

    internal void SpawnJewelry()
    {
        jewelryCtr.Reset(Vector3.zero);
    }
}
