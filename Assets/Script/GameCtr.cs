using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtr : MonoBehaviour
{
   
    void Start()
    {
        StartCoroutine(GameIE());
    }

    private IEnumerator GameIE()
    {
        yield return null;
        JewelrySystem.instance.ReSpawnJewelry();
    }
}
